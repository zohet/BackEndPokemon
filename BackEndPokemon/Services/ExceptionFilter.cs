using BackEndPokemon.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace BackEndPokemon.Services
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Response result = new Response();
            result.Success = 0;

            if (context.Exception is Exception)
            {
                var exception = context.Exception;
                result.Message = exception.InnerException != null ? exception.InnerException.Message : exception.Message;
                result.Data = exception;
            }
            else
                result.Message = "ExceptionManagerFilter";
            context.Result = new JsonResult(result);
        }
    }
}
