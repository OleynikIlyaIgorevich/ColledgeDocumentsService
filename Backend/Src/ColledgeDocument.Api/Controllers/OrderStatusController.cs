namespace ColledgeDocument.Api.Controllers;

public class OrderStatusController : BaseController
{
    public OrderStatusController(
        ColledgeDocumentDbContext dbContext)
        : base(dbContext)
    {
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var orderStatuses = await _dbContext.OrderStatuses
            .ToListAsync(cancellationToken);

        var orderStatusesResponse = orderStatuses
            .Select(orderStatus => 
                new OrderStatusResponse(
                    orderStatus.Id,
                    orderStatus.Title)).ToList();

        return Ok(orderStatusesResponse);
    }
}
