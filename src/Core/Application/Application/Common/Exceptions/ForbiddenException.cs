

using System.Net;

namespace Application.Common.Exceptions;

/// <summary>
/// 禁止访问异常 
/// </summary>
public class ForbiddenException : CustomException
{
    public ForbiddenException(string message)
        : base(message, null, HttpStatusCode.Forbidden)
    {
    }
}