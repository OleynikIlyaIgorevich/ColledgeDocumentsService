namespace ColledgeDocument.Shared.Responses;

public record UserResponse(
    [property: JsonPropertyName("user_id")]
    int Id,
    [property: JsonPropertyName("lastname")]
    string Lastname,
    [property: JsonPropertyName("firstname")]
    string Firstname,
    [property: JsonPropertyName("middlename")]
    string? Middlename,
    [property: JsonPropertyName("phone")]
    string Phone,
    [property: JsonPropertyName("username")]
    string Username,
    [property: JsonPropertyName("role")]
    string Role);