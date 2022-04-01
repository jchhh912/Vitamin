

namespace Infrastructure.Auth.Jwt;

public class JwtSettings
{

    /// <summary>
    /// 密钥
    /// </summary>
    public string? Key { get; set; }
    /// <summary>
    /// 发放人
    /// </summary>
    public string? Issuer { get; set; }
    /// <summary>
    /// Jwt持续时间
    /// </summary>
    public double TokenExpirationInMinutes { get; set; }
    /// <summary>
    /// 刷新令牌时间
    /// </summary>
    public int RefreshTokenExpirationInDays { get; set; }
}
