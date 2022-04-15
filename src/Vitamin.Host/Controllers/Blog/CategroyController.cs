using Application.Blog;
using Application.Blog.Request;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vitamin.Host.Controllers.Blog
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategroyController : BaseApiController
    {

        [HttpPost]
        public Task<Guid> CreateAsync(CreatePostCommand request)
        {
            return Mediator.Send(request);
        }
        [HttpPost]
        public Task<Guid> UpdateAsync(UpdateCategroyCommand request)
        {
            return Mediator.Send(request);
        }
        [HttpPost]
        public Task<bool> DeleteAsync(DeleteCategroyCommand request)
        {
            return Mediator.Send(request);
        }
        [HttpPost] 
        public Task<List<CreateCategoryRequest>> GetAsync(GetCategoryCommand<CreateCategoryRequest> request)
        {
            return Mediator.Send(request);
        }
        [HttpPost]
        public Task<CreateCategoryRequest> GetByIdAsync(GetSingleCategoryCommand<CreateCategoryRequest> request)
        {
            return Mediator.Send(request);
        }
    }
}
