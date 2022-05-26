using Application.Blog;
using Application.Blog.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vitamin.Host.Controllers.Blog
{

    [Authorize]
    public class CategroyController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var cats = await Mediator.Send(new GetCategorysQueryCommand());
            return Ok(cats);
        }
        [HttpDelete]
        public async Task<IActionResult> DelCategroy(string DisplayName)
        {
            var cats = await Mediator.Send(new DeleteCategoryCommand(DisplayName));
            return Ok(cats);
        }
    }
}
