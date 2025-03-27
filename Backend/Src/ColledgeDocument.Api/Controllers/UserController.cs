namespace ColledgeDocument.Api.Controllers;

public class UserController : BaseController
{
    public UserController(
        ColledgeDocumentDbContext dbContext)
        : base(dbContext)
    {
    }

    [HttpGet]
    [Authorize(Roles = "Администратор")]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<PaginationResponse<UserResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginatedAsync(
        int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await _dbContext.Users.CountAsync(cancellationToken);

        var users = await _dbContext.Users
            .Include(u => u.Role)
            .OrderBy(u => u.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var isHaveNextPage = totalCount > pageNumber * pageSize;
        var isHavePrevPage = pageNumber > 1;

        var usersResponse = users
            .Select(user =>
                new UserResponse(
                    user.Id,
                    user.Lastname,
                    user.Firstname,
                    user.Middlename,
                    user.Phone,
                    user.Username,
                    user.Role.Title)).ToList();

        var paginatedResponse = new PaginationResponse<UserResponse>(
            totalCount,
            usersResponse,
            isHaveNextPage,
            isHavePrevPage);

        return Ok(paginatedResponse);
    }

    [HttpGet("{userId:int}")]
    [Authorize(Roles = "Администратор")]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        if (userId == default) return BadRequest("Невалидный идентификатор!");

        var user = await _dbContext.Users
            .Include(u => u.Role)
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
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

    [HttpPost]
    [Authorize(Roles = "Администратор")]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<UserResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var isRequestValid = true;
        if (!isRequestValid) return BadRequest();

        var isExistByPhone = await _dbContext.Users.AnyAsync(u => u.Phone == request.Phone, cancellationToken);
        if (isExistByPhone) return BadRequest("Пользователь с данным номером телефона уже существует!");

        var isExistByUsername = await _dbContext.Users.AnyAsync(u => u.Username == request.Username, cancellationToken);
        if (isExistByUsername) return BadRequest("Пользователь с данным именем пользователя уже существует!");

        var role = await _dbContext.Roles.SingleOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);
        if (role == null) return BadRequest("Роль не найдена!");

        var user = new User()
        {
            Lastname = request.LastName,
            Firstname = request.FirstName,
            Middlename = request.MiddleName,
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            RoleId = role.Id,
        };

        var entityCreation = await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var userResponse = new UserResponse(
            entityCreation.Entity.Id,
            entityCreation.Entity.Lastname,
            entityCreation.Entity.Firstname,
            entityCreation.Entity.Middlename,
            entityCreation.Entity.Phone,
            entityCreation.Entity.Username,
            role.Title);

        return CreatedAtAction(nameof(GetByIdAsync), new { userId = userResponse.Id }, userResponse);
    }


    [HttpPut("{userId:int}")]
    [Authorize(Roles = "Администратор")]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateUserAsync(
        int userId,
        UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        if (userId == default) return BadRequest("Невалидный идентификатор!");

        var isRequestValid = true;
        if (!isRequestValid) return BadRequest();

        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null) return NotFound("Пользователь не найден!");

        var isExistForUpdateByPhone = await _dbContext.Users.AnyAsync(u => u.Id != user.Id && u.Phone == request.Phone, cancellationToken);
        if (isExistForUpdateByPhone) return BadRequest("Пользователь с данным номером телефона уже существует!");

        var isExistForUpdateByUsername = await _dbContext.Users.AnyAsync(u => u.Id != user.Id && u.Username == request.Username, cancellationToken);
        if (isExistForUpdateByUsername) return BadRequest("Пользователь с данным именем пользователя уже существует!");

        var role = await _dbContext.Roles.SingleOrDefaultAsync(r => r.Id == request.RoleId);
        if (role == null) return BadRequest("Роль не найдена!");

        user.Lastname = request.LastName;
        user.Firstname = request.FirstName;
        user.Middlename = request.MiddleName;
        user.Username = request.Username;
        if (request.IsChangePassword) user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.RoleId = role.Id;
        user.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{userId:int}")]
    [Authorize(Roles = "Администратор")]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        if (userId == default) return BadRequest("Невалидный идентификатор!");

        var currentUser = User;
        var currentUserId = Convert.ToInt32(currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var isSelfDelete = userId == currentUserId;
        if (isSelfDelete) return BadRequest("Нельзя удалить самого себя!");

        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null) return NotFound("Пользователь не найден!");

        _dbContext.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
