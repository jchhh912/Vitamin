
using System.Net;
using System.Text.Json;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Infrastructure.Middleware.Exceptions;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var errorResult = new ErrorResult
            {
                Source = ex.TargetSite?.DeclaringType?.FullName,
                Exception = ex.Message.Trim(),
                ErrorId = Guid.NewGuid().ToString(),
                SupportMessage = "exceptionmiddleware.supportmessage"
            };
            errorResult.Messages.Add(ex.Message);
            if (ex is not CustomException && ex.InnerException!=null)
            {
                while (ex.InnerException!=null)
                {
                    ex = ex.InnerException;
                }
            }
            switch (ex)
            {
                case CustomException e:
                    errorResult.StatusCode = e.StatusCode;
                    if (e.ErrorsMessages is not null)
                    {
                        errorResult.Messages = e.ErrorsMessages;
                    }
                    break;
                case KeyNotFoundException:
                    errorResult.StatusCode = HttpStatusCode.NotFound;
                    break;
                default:
                    errorResult.StatusCode=HttpStatusCode.InternalServerError;
                    break;
            }
            Log.Error($"{errorResult.Exception} Request failed with Status Code {context.Response.StatusCode} and error Id {errorResult.ErrorId}");
            var response = context.Response;
            if (!response.HasStarted) 
            {
                response.ContentType="application/json";
                response.StatusCode = (int)errorResult.StatusCode;
                string result = JsonSerializer.Serialize(errorResult);
                await response.WriteAsync(result);
            }
            else
            {
                Log.Warning("Can't write error response. Response has already started.");
            }
        }
    }
}
