using Application.Identity.Requests;
using Application.Identity.Responses;
using Application.Identity.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vitamin.Host.Controllers.Identity
{
    [AllowAnonymous]
    public class TokensController:BaseApiController
    {
        private readonly ITokenService _tokenService;
        public TokensController(ITokenService tokenService) => _tokenService = tokenService;

        [HttpPost]
        public Task<TokenResponse> GetTokenAsync(TokenRequest request, CancellationToken cancellationToken)
        {
            return _tokenService.GetTokenAsync(request, GetIpAddress(), cancellationToken);
        }
        [HttpPost("refresh")]
        public Task<TokenResponse> RefreshAsync(RefreshTokenRequest request)
        {
            return _tokenService.RefreshTokenAsync(request, GetIpAddress());
        }
        private string GetIpAddress() =>
    Request.Headers.ContainsKey("X-Forwarded-For")
        ? Request.Headers["X-Forwarded-For"]
        : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
    }
}
