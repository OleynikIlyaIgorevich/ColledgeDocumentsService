namespace ColledgeDocument.Shared.Responses;

public record OrderStatusResponse(
    [property: JsonPropertyName("order_status_id")]
    int Id,
    [property: JsonPropertyName("title")]
    string Title);