

using System.Net;

namespace Application.Common.Exceptions;

/// <summary>
/// 目标不存在异常
/// </summary>
public class NotFoundException : CustomException
{
    public NotFoundException(string message)
        : base(message, null, HttpStatusCode.NotFound)
    {
    }
}
