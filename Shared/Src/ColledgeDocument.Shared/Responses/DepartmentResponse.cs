namespace ColledgeDocument.Shared.Responses;

public record DepartmentResponse(
    [property: JsonPropertyName("department_id")]
    int Id,
    [property: JsonPropertyName("title")]
    string Title);