
using System.Net;

namespace Application.Common.Exceptions;

public class ConflictException : CustomException
{
    /// <summary>
    /// 冲突异常：重复冲突
    /// </summary>
    /// <param name="message"></param>
    public ConflictException(string message)
        : base(message, null, HttpStatusCode.Conflict)
    {
    }
}
