

using Application.Common.Interfaces;
using Application.Identity.Requests;

namespace Application.Identity.Users;

public interface IUsersService : ITransientService
{
    Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken);
}