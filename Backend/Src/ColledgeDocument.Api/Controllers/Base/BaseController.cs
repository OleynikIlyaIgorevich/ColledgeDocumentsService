namespace ColledgeDocument.Api.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected readonly ColledgeDocumentDbContext _dbContext;

    public BaseController(
        ColledgeDocumentDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}
