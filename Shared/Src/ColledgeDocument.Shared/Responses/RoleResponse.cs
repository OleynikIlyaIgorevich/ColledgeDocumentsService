namespace ColledgeDocument.Shared.Responses;

public record RoleResponse(
    [property: JsonPropertyName("role_id")]
    int Id,
    [property: JsonPropertyName("title")]
    string Title);