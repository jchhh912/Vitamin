

using Application.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vitamin.Host.Controllers.Blog;


public class PostController : BaseApiController
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<Guid> CreateAsync(CreatePostCommand request) 
    {
        return await Mediator.Send(request);
    }
    [HttpDelete]
    [AllowAnonymous]
    public async Task<Guid> DeleteAsync(DeletePostCommand request)
    {
        return await Mediator.Send(request);
    }
}
