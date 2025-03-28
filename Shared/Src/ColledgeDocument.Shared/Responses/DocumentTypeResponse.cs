namespace ColledgeDocument.Shared.Responses;

public record DocumentTypeResponse(
    [property: JsonPropertyName("document_type_id")]
    int Id,
    [property: JsonPropertyName("title")]
    string Title);