
namespace ColledgeDocument.Api.Controllers;

public class DocumentTypeController : BaseController
{
    public DocumentTypeController(
        ColledgeDocumentDbContext dbContext)
        : base(dbContext)
    {
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<List<DocumentTypeResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var documentTypes = await _dbContext.DocumentTypes.ToListAsync(cancellationToken);
        var documentTypesResponse = documentTypes
            .Select(documentType => new DocumentTypeResponse(
                documentType.Id,
                documentType.Title)).ToList();

        return Ok(documentTypesResponse);
    }

    [HttpGet("{documentTypeId:int}")]
    [Authorize]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<DocumentTypeResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(
        int documentTypeId,
        CancellationToken cancellationToken = default)
    {
        if (documentTypeId == default) return BadRequest("Идентификатор не валиден!");

        var documentType = await _dbContext.DocumentTypes.SingleOrDefaultAsync(cancellationToken);
        if (documentType == null) return NotFound("Тип справки не найдена!");

        var documentTypeResponse = new DocumentTypeResponse(
            documentType.Id,
            documentType.Title);

        return Ok(documentTypeResponse);
    }

    [HttpPost]
    [Authorize(Roles = "Администратор")]
    [ProducesResponseType<string>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<DocumentTypeResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync(
        CreateDocumentTypeRequest request,
        CancellationToken cancellationToken = default)
    {
        var isRequestValid = true;
        if (!isRequestValid) return BadRequest();

        var isExistByTitle = await _dbContext.DocumentTypes.AnyAsync(x => x.Title == request.Title, cancellationToken);
        if (isExistByTitle) return BadRequest("Тип справки с таким названием уже существует!");

        var documentType = new DocumentType()
        {
            Title = request.Title,
        };

        var entityCreation = await _dbContext.DocumentTypes.AddAsync(documentType, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var documentTypeResponse = new DocumentTypeResponse(
            entityCreation.Entity.Id,
            entityCreation.Entity.Title);

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { documentTypeId = documentTypeResponse.Id },
            documentTypeResponse);
    }

    [HttpPut("{documentTypeId:int}")]
    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> UpdateAsync(
        int documentTypeId,
        UpdateDocumentTypeRequest request,
        CancellationToken cancellationToken = default)
    {
        if (documentTypeId == default) return BadRequest("Идентификатор не валиден!");

        var isRequestValid = true;
        if (!isRequestValid) return BadRequest();

        var isExistByTitleForUpdate = await _dbContext.DocumentTypes
            .AnyAsync(x => x.Id != documentTypeId && x.Title == request.Title, cancellationToken);
        if (isExistByTitleForUpdate) return BadRequest("Тип справки с данным названием уже существует!");

        var documentType = await _dbContext.DocumentTypes.SingleOrDefaultAsync(x => x.Id == documentTypeId, cancellationToken);
        if (documentType == null) return NotFound("Тип справки с данным названием не найден!");

        documentType.Title = request.Title;
        documentType.UpdatedAt = DateTime.Now;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }


    [HttpDelete("{documentTypeId:int}")]
    [Authorize(Roles = "Администратор")]
    public async Task<IActionResult> DeleteAsync(
        int documentTypeId,
        CancellationToken cancellationToken = default)
    {
        if (documentTypeId == default) return BadRequest("Идентификатор не валиден!");

        var isRequestValid = true;
        if (!isRequestValid) return BadRequest();

        var documentType = await _dbContext.DocumentTypes.SingleOrDefaultAsync(x => x.Id == documentTypeId, cancellationToken);
        if (documentType == null) return NotFound("Тип справки с данным названием не найден!");

        _dbContext.DocumentTypes.Remove(documentType);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}
