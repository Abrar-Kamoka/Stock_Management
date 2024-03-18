using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AcmeStockApp.Controllers
{


    public class ErrorController : IExceptionFilter
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        // [Route("Error")]
        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                var exception = context.Exception;


                int statusCode;


                switch (true)
                {
                    case bool _ when exception is UnauthorizedAccessException:
                        statusCode = (int)HttpStatusCode.Unauthorized;
                        break;


                    case bool _ when exception is InvalidOperationException:
                        statusCode = (int)HttpStatusCode.BadRequest;
                        break;


                    default:
                        statusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }


                logger.LogError($"GlobalExceptionFilter: Error in {context.ActionDescriptor.DisplayName}. {exception.Message}. Stack Trace: {exception.StackTrace}");

                context.Result = new ObjectResult(exception.Message) { StatusCode = statusCode };

                //context.HttpContext.Response.Redirect("/Error/GlobalError");
            }
        }
    }
}