namespace ColledgeDocument.Api.Controllers;

public class DocumentOrderController : BaseController
{
    public DocumentOrderController(
        ColledgeDocumentDbContext dbContext)
        : base(dbContext)
    {
    }

    [HttpGet]
    [Authorize(Roles = "Оператор справок, Администратор")]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<PaginationResponse<DocumentOrderResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginatedAsync(
        int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await _dbContext.DocumentOrders.CountAsync(cancellationToken);

        var documentOrders = await _dbContext.DocumentOrders
            .Include(u => u.User)
            .Include(dr => dr.DocumentType)
            .Include(dr => dr.OrderStatus)
            .Include(dr => dr.Departament)
            .OrderBy(u => u.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var isHaveNextPage = totalCount > pageNumber * pageSize;
        var isHavePrevPage = pageNumber > 1;

        var documentOrdersResponse = documentOrders
            .Select(documentOrder =>
                new DocumentOrderResponse(
                    documentOrder.Id,
                    documentOrder.User.Lastname,
                    documentOrder.User.Firstname,
                    documentOrder.User.Middlename,
                    documentOrder.User.Username,
                    documentOrder.DocumentType.Title,
                    documentOrder.Departament.Title,
                    documentOrder.OrderStatus.Title,
                    documentOrder.Quantity,
                    documentOrder.CreatedAt,
                    documentOrder.UpdatedAt)).ToList();

        var paginatedResponse = new PaginationResponse<DocumentOrderResponse>(
            totalCount,
            documentOrdersResponse,
            isHaveNextPage,
            isHavePrevPage);

        return Ok(paginatedResponse);
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<PaginationResponse<DocumentOrderResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaginatedMeAsync(
        int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var currentUser = User;
        var currentUserId = Convert.ToInt32(currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var totalCount = await _dbContext.DocumentOrders.CountAsync(cancellationToken);

        var documentOrders = await _dbContext.DocumentOrders
            .Include(x => x.User)
            .Include(x => x.DocumentType)
            .Include(x => x.OrderStatus)
            .Include(x => x.Departament)
            .Where(x => x.UserId == currentUserId)
            .OrderBy(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
            .ToListAsync(cancellationToken);

        var isHaveNextPage = totalCount > pageNumber * pageSize;
        var isHavePrevPage = pageNumber > 1;

        var documentOrdersResponse = documentOrders
            .Select(documentOrder =>
                new DocumentOrderResponse(
                    documentOrder.Id,
                    documentOrder.User.Lastname,
                    documentOrder.User.Firstname,
                    documentOrder.User.Middlename,
                    documentOrder.User.Username,
                    documentOrder.DocumentType.Title,
                    documentOrder.Departament.Title,
                    documentOrder.OrderStatus.Title,
                    documentOrder.Quantity,
                    documentOrder.CreatedAt,
                    documentOrder.UpdatedAt)).ToList();

        var paginatedResponse = new PaginationResponse<DocumentOrderResponse>(
            totalCount,
            documentOrdersResponse,
            isHaveNextPage,
            isHavePrevPage);

        return Ok(paginatedResponse);
    }

    [HttpGet("{documentOrderId:int}")]
    [Authorize]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<DocumentOrderResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(
        int documentOrderId,
        CancellationToken cancellationToken = default)
    {
        if (documentOrderId == default) return BadRequest("Некорректный идентификатор!");

        var documentOrder = await _dbContext.DocumentOrders
            .Include(x => x.User)
            .Include(x => x.DocumentType)
            .Include(x => x.OrderStatus)
            .Include(x => x.Departament)
            .SingleOrDefaultAsync(x => x.Id == documentOrderId, cancellationToken);
        if (documentOrder == null) return NotFound("Заявка на справку не найдена!");

        var documentOrderResponse = new DocumentOrderResponse(
            documentOrder.Id,
            documentOrder.User.Lastname,
            documentOrder.User.Firstname,
            documentOrder.User.Middlename,
            documentOrder.User.Username,
            documentOrder.DocumentType.Title,
            documentOrder.Departament.Title,
            documentOrder.OrderStatus.Title,
            documentOrder.Quantity,
            documentOrder.CreatedAt,
            documentOrder.UpdatedAt);

        return Ok(documentOrderResponse);
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<UserResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync(
        CreateDocumentOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUser = User;
        var currentUserId = Convert.ToInt32(currentUser.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var isRequestValid = true;
        if (!isRequestValid) return BadRequest();

        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == currentUserId, cancellationToken);
        if (user == null) return NotFound("Пользователь не найден!");

        var orderStatus = await _dbContext.OrderStatuses.SingleOrDefaultAsync(x => x.Id == 1, cancellationToken);
        if (orderStatus == null) return NotFound("Статус не найден!");

        var documentType = await _dbContext.DocumentTypes.SingleOrDefaultAsync(x => x.Id == request.DocumentTypeId, cancellationToken);
        if (documentType == null) return NotFound("Тип документа не найден!");

        var departament = await _dbContext.Departaments.SingleOrDefaultAsync(x => x.Id == request.DepartamnetId, cancellationToken);
        if (departament == null) return NotFound("Подразделение не найдено!");

        var documentOrder = new DocumentOrder()
        {
            UserId = user.Id,
            DocumentTypeId = documentType.Id,
            OrderStatusId = orderStatus.Id,
            DepartamentId = departament.Id,
            Quantity = request.Quantity,
        };

        var entityCreation = await _dbContext.DocumentOrders.AddAsync(documentOrder, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var documentOrderResponse = new DocumentOrderResponse(
            entityCreation.Entity.Id,
            user.Lastname,
            user.Firstname,
            user.Middlename,
            user.Username,
            documentType.Title,
            departament.Title,
            orderStatus.Title,
            entityCreation.Entity.Quantity,
            entityCreation.Entity.CreatedAt,
            entityCreation.Entity.UpdatedAt);

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { documentOrderId = documentOrderResponse.Id },
            documentOrderResponse);
    }
}
