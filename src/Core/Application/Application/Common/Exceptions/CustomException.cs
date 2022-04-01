
using System.Net;

namespace Application.Common.Exceptions;

/// <summary>
/// 自定义异常
/// </summary>
public  class CustomException : Exception
{
    public List<string>? ErrorsMessages { get; }
    public HttpStatusCode StatusCode { get; }

    public CustomException(string message, List<string>? errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        ErrorsMessages = errors;
        StatusCode = statusCode;
    }
}
