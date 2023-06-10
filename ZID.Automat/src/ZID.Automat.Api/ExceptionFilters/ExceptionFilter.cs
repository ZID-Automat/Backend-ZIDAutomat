using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using System.Net.Http;
using ZID.Automat.Dto.Models;
using ZID.Automat.Exceptions;

namespace ZID.Automat.Api.ExceptionFilters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is PasswordWrongException)
            {
                context.Result = new UnauthorizedObjectResult(new ErrorResponseDto() { Message = context.Exception.Message, StatusCode = 401 });
            }
            else if (context.Exception is NotFoundException)
            {
                context.Result = new NotFoundObjectResult(new ErrorResponseDto() { Message = context.Exception.Message, StatusCode = 404 });
            }
            else if (context.Exception is BorrowDueTimeInvalidException || context.Exception is NoItemAvailable || context.Exception is QrCodeNotExistingException)
            {
                context.Result = new BadRequestObjectResult(new ErrorResponseDto() { Message = context.Exception.Message, StatusCode = 400 });
            }
            else if (context.Exception is ZuVielUnbehandelteBorros)
            {
                context.Result = new BadRequestObjectResult(new ErrorResponseDto() { Message = context.Exception.Message, StatusCode = 400 });
            }
            else
            {
                throw context.Exception;
            }
        }
    }
}

