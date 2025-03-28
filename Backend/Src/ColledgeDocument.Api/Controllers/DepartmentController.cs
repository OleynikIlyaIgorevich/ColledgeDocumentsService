namespace ColledgeDocument.Api.Controllers;

public class DepartmentController : BaseController
{
    public DepartmentController(
        ColledgeDocumentDbContext dbContext)
        : base(dbContext)
    {
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<List<DepartmentResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var departments = await _dbContext.Departaments.ToListAsync(cancellationToken);
        var departmentsResponse = departments
            .Select(department => new DepartmentResponse(
                department.Id,
                department.Title)).ToList();

        return Ok(departmentsResponse);
    }

    [HttpGet("{departmentId:int}")]
    [Authorize]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<DepartmentResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(
        int departmentId,
        CancellationToken cancellationToken = default)
    {
        if (departmentId == default) return BadRequest("Идентификатор не валиден!");

        var department = await _dbContext.Departaments
            .SingleOrDefaultAsync(x => x.Id == departmentId, cancellationToken);
        if (department == null) return NotFound("Отделение не найдено!");

        var departmentResponse = new DepartmentResponse(
            department.Id,
            department.Title);

        return Ok(departmentResponse);
    }

    [HttpPost]
    [Authorize(Roles = "Администратор")]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<DepartmentResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var isRequestValid = true;
        if (!isRequestValid) return BadRequest();

        var isExistByTitle = await _dbContext.Departaments
            .AnyAsync(x => x.Title == request.Title, cancellationToken);
        if (isExistByTitle) return BadRequest("Отдел с данным названием уже существует!");

        var department = new Departament
        {
            Title = request.Title,
        };

        var entityCreation = await _dbContext.Departaments.AddAsync(department, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var departmentResponse = new DepartmentResponse(
            entityCreation.Entity.Id,
            entityCreation.Entity.Title);

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { departmentId = departmentResponse.Id },
            departmentResponse);
    }

    [HttpPut("{departmentId:int}")]
    [Authorize(Roles = "Администратор")]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateAsync(
        int departmentId,
        UpdateDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        if (departmentId == default) return BadRequest("Идентификатор не валиден!");

        var isRequestValid = true;
        if (!isRequestValid) return BadRequest();

        var isExistByTitleForUpdate = await _dbContext.Departaments
            .AnyAsync(x => x.Id != departmentId && x.Title == request.Title, cancellationToken);
        if (isExistByTitleForUpdate) return BadRequest("Отдел с данным названием уже существует!");

        var department = await _dbContext.Departaments
            .SingleOrDefaultAsync(x => x.Id == departmentId, cancellationToken);
        if (department == null) return NotFound("Отдел не найден!");

        department.Title = request.Title;
        department.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{departmentId:int}")]
    [Authorize(Roles = "Администратор")]
    [Authorize(Roles = "Администратор")]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync(
        int departmentId,
        CancellationToken cancellationToken = default)
    {
        if (departmentId == default) return BadRequest("Идентификатор не валиден!");

        var department = await _dbContext.Departaments
          .SingleOrDefaultAsync(x => x.Id == departmentId, cancellationToken);
        if (department == null) return NotFound("Отдел не найден!");

        _dbContext.Departaments.Remove(department);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }


}
