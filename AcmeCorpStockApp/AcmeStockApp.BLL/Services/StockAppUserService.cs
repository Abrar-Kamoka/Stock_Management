using AcmeStockApp.BLL.Interfaces;
using AcmeStockApp.DAL.Interfaces;
using AcmeStockApp.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Collections.Generic;

namespace AcmeStockApp.BLL.Services
{
    public class StockAppUserService : IStockAppUserService
    {
        private readonly IStockAppUserRepository _userRepo;

        public object HttpContext { get; private set; }

        public StockAppUserService(IStockAppUserRepository appUserRepository)
        {
            _userRepo = appUserRepository;
        }

        public StockAppUser CheckByEmail(string email)
        {
            return _userRepo.CheckUserByEmail(email);
        }

        public StockAppUser CheckByPassword(string password)
        {
            return _userRepo.CheckUserByPassword(password);
        }

        public StockAppUser CheckUserByEmailandPassword(string email, string password)
        {
            return _userRepo.CheckByEmailandPassword(email, password);
        }
        public IEnumerable<StockAppUser> GetAll()
        {
            return _userRepo.GetAll();
        }

        public StockAppUser Add(StockAppUser regUser)
        {
            return _userRepo.AddUser(regUser);
        }
        public void Save()
        {
            _userRepo.Save();
        }
        public StockAppUser Update(StockAppUser user)
        {
            _userRepo.Update(user);
            return user;
        }

        public void SendEmail(string email, string subject, string messageBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Acme Stock App", "farwamuqaddas214@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = messageBody };

            using var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            client.Authenticate("farwamuqaddas214@gmail.com", "qbazdnmqesjvmxpw");
            client.Send(message);
            client.Disconnect(true);
        }



        //public StockAppUser Update(StockAppUser Password, StockAppUser NewPassword)
        //{

        //}

        //public StockAppUser ChangePass(StockAppUser oldPas, StockAppUser newPas)
        //{
        //    oldPas.Email == newPas.Email;
        //    return oldPas;
        //}
    }
}
