

using Application.Common.Exceptions;
using Application.Identity.Requests;
using Application.Identity.Responses;
using Application.Identity.Tokens;
using Infrastructure.Auth.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Identity;

public class TokenService:ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;
    public TokenService(
       UserManager<ApplicationUser> userManager,
       IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }
    /// <summary>
    /// 获取Token
    /// </summary>
    /// <exception cref="UnauthorizedException"></exception>
    public async Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email.Trim().Normalize());
        if (user is null)
        {
            throw new UnauthorizedException("auth.failed");
        }
        if (!user.IsActive)
        {
            throw new UnauthorizedException("identity.usernotactive");
        }
        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedException("identity.invalidcredentials");
        }
        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }
    /// <summary>
    /// 刷新Token更新
    /// </summary>
    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        string? userEmail = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userEmail);
        if (user is null)
        {
            throw new UnauthorizedException("auth.failed");
        }

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("identity.invalidrefreshtoken");
        }

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    #region JWT辅助方法
    /// <summary>
    /// 生成Token并且更新
    /// </summary>
    /// <param name="user"></param>
    /// <param name="ipAddress"></param>
    /// <returns></returns>
    private async Task<TokenResponse> GenerateTokensAndUpdateUser(ApplicationUser user, string ipAddress)
    {
        string token = GenerateJwt(user, ipAddress);

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

        await _userManager.UpdateAsync(user);

        return new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
    }
    /// <summary>
    /// 生成JWT
    /// </summary>
    /// <returns></returns>
    private string GenerateJwt(ApplicationUser user, string ipAddress) =>
    GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));
    /// <summary>
    /// 获取身份信息
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<Claim> GetClaims(ApplicationUser user, string ipAddress) =>
       new List<Claim>
       {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(VitaminClaims.Fullname,user.UserName),
            new(VitaminClaims.IpAddress, ipAddress)
       };
    /// <summary>
    /// 生成刷新令牌
    /// </summary>
    /// <returns></returns>
    private static string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    /// <summary>
    /// 生成加密令牌
    /// </summary>
    /// <param name="signingCredentials">签名凭证</param>
    /// <param name="claims">身份信息</param>
    /// <returns></returns>
    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           issuer: _jwtSettings.Issuer,
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
    /// <summary>
    /// 获取签名凭证
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private SigningCredentials GetSigningCredentials()
    {
        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            throw new InvalidOperationException("No Key defined in JwtSettings config.");
        }

        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }
    /// <summary>
    /// 解析令牌
    /// </summary>
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            throw new InvalidOperationException("No Key defined in JwtSettings config.");
        }

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = true,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new UnauthorizedException("identity.invalidtoken");
        }

        return principal;
    }
    #endregion
}
