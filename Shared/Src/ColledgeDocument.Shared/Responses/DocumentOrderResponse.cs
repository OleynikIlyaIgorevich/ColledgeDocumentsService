namespace ColledgeDocument.Shared.Responses;

public record DocumentOrderResponse(
    [property: JsonPropertyName("document_order_id")]
    int Id,
    [property: JsonPropertyName("lastname")]
    string Lastname,
    [property: JsonPropertyName("firstname")]
    string Firstname,
    [property: JsonPropertyName("middlename")]
    string? Middlename,
    [property: JsonPropertyName("username")]
    string Username,
    [property: JsonPropertyName("document_type")]
    string DocumentType,
    [property: JsonPropertyName("departament")]
    string Departament,
    [property: JsonPropertyName("order_status")]
    string OrderStatus,
    [property: JsonPropertyName("quantity")]
    int Quantity,
    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt,
    [property: JsonPropertyName("updated_at")]
    DateTime? UpdatedAt);