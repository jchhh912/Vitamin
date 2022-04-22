using Application.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vitamin.Host.Controllers.Blog;

[AllowAnonymous]
public class TagsController:BaseApiController
{
    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        var cats = await Mediator.Send(new GetTagsQueryCommand());
        return Ok(cats);
    }
}
