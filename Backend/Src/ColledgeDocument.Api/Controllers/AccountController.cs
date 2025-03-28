namespace ColledgeDocument.Api.Controllers;

public class AccountController : BaseController
{
    public AccountController(
        ColledgeDocumentDbContext dbContext)
        : base(dbContext)
    {
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMeAsync(
        CancellationToken cancellationToken = default)
    {
        var currentUser = User;
        var currentUserId = Convert.ToInt32(currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var user = await _dbContext.Users
            .Include(u => u.Role)
            .SingleOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);
        if (user == null) return NotFound("Пользователь не найден!");

        var userResponse = new UserResponse(
            user.Id,
            user.Lastname,
            user.Firstname,
            user.Middlename,
            user.Phone,
            user.Username,
            user.Role.Title);

        return Ok(userResponse);
    }

    [HttpPut("update-profile")]
    [Authorize]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateProfileAsync(
        UpdateProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUser = User;
        var currentUserId = Convert.ToInt32(currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var user = await _dbContext.Users
            .Include(u => u.Role)
            .SingleOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);
        if (user == null) return NotFound("Пользователь не найден!");

        var isRequestValid = true;
        if (!isRequestValid) return BadRequest();

        user.Lastname = request.Lastname;
        user.Firstname = request.Firstname;
        user.Middlename = request.Middlename;
        user.Phone = request.Phone;
        user.Username = request.Username;
        user.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpPut("update-password")]
    [Authorize]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdatePasswordAsync(
        UpdatePasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUser = User;
        var currentUserId = Convert.ToInt32(currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var user = await _dbContext.Users
            .Include(u => u.Role)
            .SingleOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);
        if (user == null) return NotFound("Пользователь не найден!");

        var isRequestValid = true;
        if (!isRequestValid) return BadRequest();

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
