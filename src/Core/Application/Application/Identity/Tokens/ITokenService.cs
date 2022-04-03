

using Application.Common.Interfaces;
using Application.Identity.Requests;
using Application.Identity.Responses;

namespace Application.Identity.Tokens;

public interface ITokenService:ITransientService
{
    Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken);

    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
}
