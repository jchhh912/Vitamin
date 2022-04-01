

using System.Net;

namespace Application.Common.Exceptions;
/// <summary>
/// 服务器异常
/// </summary>
public class InternalServerException : CustomException
{
    public InternalServerException(string message, List<string>? errors = default)
        : base(message, errors, HttpStatusCode.InternalServerError)
    {
    }
}