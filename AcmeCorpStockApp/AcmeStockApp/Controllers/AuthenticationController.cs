using AcmeStockApp.BLL.Interfaces;
using AcmeStockApp.DTOs;
using AcmeStockApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static AcmeStockApp.DTOs.UserLoginDTO;

namespace AcmeStockApp.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IStockAppUserService _userService;
        
        public AuthenticationController(IStockAppUserService userContext)
        {
            _userService = userContext;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {

            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.ErrorMessage = null;
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginDTO userLoginDTO, string gRecaptchaResponse, bool rememberMe)
        {
            RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();
            if (String.IsNullOrEmpty(recaptchaHelper.Response))
            {
                ModelState.AddModelError("", "Captcha answer cannot be empty.");
                return View(gRecaptchaResponse);
            }
            RecaptchaVerificationResult recaptchaResult = recaptchaHelper.VerifyRecaptchaResponse();
            if (!recaptchaResult.Success)
            {
                ModelState.AddModelError("", "Incorrect captcha answer.");
            }

            var myUser = _userService.CheckUserByEmailandPassword(userLoginDTO.Email, userLoginDTO.Password);
            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession", myUser.Email);

                if (rememberMe)
                {
                    var options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(2),
                        IsEssential = true,
                        HttpOnly = true,
                    };

                    string userData = $"{myUser.Email}|{myUser.Password}";

                    Response.Cookies.Append("UserSession", userData, options);
                }

                return RedirectToAction("Dashboard");
            }

            else
            {
                ViewBag.ErrorMessage = "Login failed.";
            }

            return View();
        }
            
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.Mysession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        public IActionResult Logout()
        {

            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }

            return View();
        }

        //Registration
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(StockAppUser regUser)
        {
            if (ModelState.IsValid)
            {
                StockAppUser existingUser = _userService.CheckByEmail(regUser.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "This email is already taken.");
                    return View(regUser);
                }

                StockAppUser existingPass = _userService.CheckByPassword(regUser.Password);
                if (existingPass != null)
                {
                    ModelState.AddModelError("Password", "This password is already taken.");
                    return View(regUser);
                }

                if (!IsValidPassword(regUser.Password))
                {
                    ModelState.AddModelError("Password", "Password must be at least 8 characters long and contain at least 1 capital letter and 1 number.");
                    return View(regUser);
                }

                if (regUser.Password != regUser.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                    return View();
                }

                //true means admin, false meas registrar 
                if (regUser.UserType == true)
                {
                    regUser.UserType = true;
                }
                else
                {
                    regUser.UserType = false;
                }
                _userService.Add(regUser);
                _userService.Save();
                TempData["Success"] = "Registered Successfully";
                return RedirectToAction("Login");
            }
            return View();
        }

        //password format
        private bool IsValidPassword(string password)
        {
            return password.Length >= 8 && password.Any(char.IsUpper) && password.Any(char.IsDigit);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(UserForgotPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                StockAppUser user = _userService.CheckByEmail(model.Email);

                if (user != null)
                {

                    var passwordResetLink = Url.Action("ForgotChangePassword", "Authentication",
                            new { email = model.Email }, Request.Scheme);

                    //email message
                    string subject = "Password Reset Request";
                    string messageBody = $"Dear {user.Name},\n\n";
                    messageBody += "Please click the following link to reset your password:\n";
                    messageBody += passwordResetLink;

                    _userService.SendEmail(user.Email, subject, messageBody);

                    return View("ForgotPasswordConfirmation");
                }

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        public ActionResult ForgotPasswordConfirmation()
        {
            TempData["Success"] = "Password emailed to you. Check your inbox or spam folder.";
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgotChangePassword(string email)
        {
            UserForgotChangePasswordDTO userForgotChangePasswordDTO = new UserForgotChangePasswordDTO { Email = email };
            return View(userForgotChangePasswordDTO);
        }
        
        [HttpPost]
        public ActionResult ForgotChangePassword(UserForgotChangePasswordDTO model)
        {
            StockAppUser existingUser = _userService.CheckByEmail(model.Email);
            if (existingUser != null)
            {

                if (!IsValidPassword(model.NewPassword))
                {
                    ModelState.AddModelError("Password", "Password must be at least 8 characters long and contain at least 1 capital letter and 1 number.");
                    return View(model);
                }

                if (model.ConfirmPassword != model.NewPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The new password and confirmation password do not match.");
                    return View(model);
                }

                existingUser.Password = model.NewPassword;
                existingUser.ConfirmPassword = model.ConfirmPassword;
                _userService.Update(existingUser);
                _userService.Save();
                TempData["Success"] = "Password Updated Successfully.";
                return RedirectToAction("Login", "Authentication");
            }

            return View(model);
        }
        
        [HttpGet]
        public ActionResult ChangePassword()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                UserChangePasswordDTO userChangePasswordDTO = new UserChangePasswordDTO { Email = HttpContext.Session.GetString("UserSession").ToString() };
                return View(userChangePasswordDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(UserChangePasswordDTO model)
        {
            StockAppUser existingUser = _userService.CheckUserByEmailandPassword(model.Email, model.Password);
            if (existingUser != null)
            {
                if (existingUser.Password != model.Password)
                {
                    ModelState.AddModelError("Password", "Incorrect current password.");
                    return View(model);
                }

                if (model.ConfirmPassword != model.NewPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The new password and confirmation password do not match.");
                    return View(model);
                }

                existingUser.Password = model.NewPassword;
                existingUser.ConfirmPassword = model.ConfirmPassword;
                _userService.Update(existingUser);
                _userService.Save();
                TempData["ChangeSuccess"] = "Password Updated Successfully.";
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        // List users
        public ActionResult ListUsers()
        {
            var userData = _userService.GetAll().ToList();

            foreach (var user in userData)
            {
                if (user.UserType != null)
                {
                    user.UserTypeString = user.UserType.Value ? "Admin" : "Registrar";
                }
            }
            return View(userData);
        }

        public IActionResult Registrar()
        {
            var currentUserEmail = HttpContext.Session.GetString("UserSession");

            var user = _userService.CheckByEmail(currentUserEmail);

            if (user == null || user.UserType == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // true (Admin)
            if (user.UserType == true)
            {
                TempData["ErrorMessage"] = "Only Registrar has access to this page.";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
