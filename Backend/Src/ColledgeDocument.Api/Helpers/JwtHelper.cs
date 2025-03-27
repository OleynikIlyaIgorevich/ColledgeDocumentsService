namespace ColledgeDocument.Api.Helpers;

public interface IJwtHepler
{
    string GenerateRefreshToken();
    string GenerateJwtToken(User user);
}

public class JwtHelper : IJwtHepler
{
    private readonly JwtOptions _jwtOptions;

    public JwtHelper(
        IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public string GenerateJwtToken(User user) =>
        GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user));

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.ExpiresMinutes),
            signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var encryptedToken = tokenHandler.WriteToken(token);
        return encryptedToken;
    }

    private IEnumerable<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, user.Role.Title),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        return claims;
    }

    private SigningCredentials GetSigningCredentials() =>
        new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);
}
