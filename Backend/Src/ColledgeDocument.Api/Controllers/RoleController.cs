
namespace ColledgeDocument.Api.Controllers;

public class RoleController : BaseController
{
    public RoleController(
        ColledgeDocumentDbContext dbContext) 
        : base(dbContext)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var roles = await _dbContext.Roles
            .ToListAsync();

        var rolesResponse = roles.Select(
            role => new RoleResponse(
                role.Id,
                role.Title)).ToList();

        return Ok(rolesResponse);
    }
}
