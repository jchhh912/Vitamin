using Application.Identity.Requests;
using Application.Identity.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vitamin.Host.Controllers.Identity;

[ApiController]
public class UsersController : BaseApiController
{
    private readonly IUsersService _userService;
    public UsersController(IUsersService userService) => _userService = userService;
    [Authorize]
    [HttpGet("{id}")]
    public Task<UserDetailsDto> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _userService.GetAsync(id, cancellationToken);
    }
}
