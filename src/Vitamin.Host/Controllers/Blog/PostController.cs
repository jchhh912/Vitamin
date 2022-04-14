

using Application.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vitamin.Host.Controllers.Blog;


public class PostController : BaseApiController
{
    [HttpPost]
    [AllowAnonymous]
    public Task<Guid> CreateAsync(CreatePostCommand request) 
    {
        return Mediator.Send(request);
    }
}
