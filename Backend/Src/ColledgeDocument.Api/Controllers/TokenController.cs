namespace ColledgeDocument.Api.Controllers;

public class TokenController : BaseController
{
    private readonly IJwtHepler _jwtHelper;

    public TokenController(
        ColledgeDocumentDbContext dbContext,
        IJwtHepler jwtHelper)
        : base(dbContext)
    {
        _jwtHelper = jwtHelper; 
    }

    [HttpPost]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> SignInAsync(
        TokenRequest request,
        CancellationToken cancellationToken = default)
    {
        var isRequestValid = true;
        if (!isRequestValid) return BadRequest();

        var user = await _dbContext.Users
            .Include(u => u.Role)
            .SingleOrDefaultAsync(
            u => u.Username == request.Username,
            cancellationToken: cancellationToken);
        if (user == null) return Unauthorized("Неправильный логин или пароль");

        var isPasswordConfirm = BCrypt.Net.BCrypt
            .Verify(request.Password, user.PasswordHash);
        if (!isPasswordConfirm) return Unauthorized("Неправильный логин или пароль");

        var refreshToken = _jwtHelper.GenerateRefreshToken();
        var accessToken = _jwtHelper.GenerateJwtToken(user);

        var session = new RefreshSession()
        {
            UserId = user.Id,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.Now.AddDays(7)
        };

        await _dbContext.RefreshSessions.AddAsync(session, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var tokenResponse = new TokenResponse(accessToken, refreshToken);

        return Ok(tokenResponse);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshTokenAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        var isRequestValid = true;
        if (!isRequestValid) return BadRequest();

        var refreshSession = await _dbContext.RefreshSessions
          .Include(rf => rf.User)
          .ThenInclude(u => u.Role)
          .SingleOrDefaultAsync(rs => rs.RefreshToken == request.RefreshToken);
        if (refreshSession == null) return Unauthorized("Токен обновления не найден!");

        var isSessionExpired = refreshSession.ExpiresAt < DateTime.Now;
        if (isSessionExpired) return Unauthorized("Сессия устарела, необходимо снова пройти авторизацию!");


        var refreshToken = _jwtHelper.GenerateRefreshToken();
        var accessToken = _jwtHelper.GenerateJwtToken(refreshSession.User);

        _dbContext.Remove(refreshSession);

        var newRefreshSession = new RefreshSession()
        {
            UserId = refreshSession.User.Id,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.Now.AddDays(7)
        };

        await _dbContext.AddAsync(newRefreshSession, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var tokenResponse = new TokenResponse(accessToken, refreshToken);

        return Ok(tokenResponse);
    }
}
