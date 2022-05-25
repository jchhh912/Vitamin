

using Application.Common.Exceptions;
using Application.Identity.Requests;
using Application.Identity.Users;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity;

public class UserService : IUsersService
{
    private readonly UserManager<ApplicationUser> _userManager;
    public UserService(UserManager<ApplicationUser> userManager) { 
        _userManager = userManager;
    }
    public async Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
           .AsNoTracking()
           .Where(u => u.Id == userId)
           .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException("User Not Found.");

        return user.Adapt<UserDetailsDto>();
    }
}
