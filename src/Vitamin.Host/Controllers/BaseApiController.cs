using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Vitamin.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private ISender _mediator = null!;
        /// <summary>
        /// 绑定当前请求的IServiceProvider对象
        /// </summary>
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
