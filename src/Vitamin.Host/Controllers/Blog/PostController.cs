

using Application.Blog;
using Application.Blog.Admin;
using Application.Blog.Request;
using Domain.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vitamin.Host.Controllers.Blog;



[Authorize]
public class PostController : BaseApiController
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<Post> GetPostByIdQuery([FromQuery] GetPostByIdQueryCommand request)
    {
        return await Mediator.Send(request);
    }
    [AllowAnonymous]
    [HttpGet("List/Published")]
    public async Task<IReadOnlyList<PostDto>> ListPublished()
    {
        return await Mediator.Send(new PublishPostCommand(Domain.Blog.PostStatus.Published));
    }
    [HttpGet("List/Deleted")]
    public async Task<IReadOnlyList<PostDto>> ListDeleted()
    {
        return await Mediator.Send(new PublishPostCommand(Domain.Blog.PostStatus.Deleted));
    }
    [HttpPost]

    public async Task<Guid> CreateAsync(CreatePostCommand request)
    {
        return await Mediator.Send(request);
    }
    [HttpDelete]
    public async Task<Guid> DeleteAsync(DeletePostCommand request)
    {
        return await Mediator.Send(request);
    }
    [HttpPut]
    public async Task<Guid> UpdateAsync(UpdatePostCommand request)
    {
        return await Mediator.Send(request);
    }
}
