namespace Application.Identity.Requests;

public record RefreshTokenRequest(string Token, string RefreshToken);