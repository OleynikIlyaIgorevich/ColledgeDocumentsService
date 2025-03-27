namespace ColledgeDocument.Shared.Requests;

public class TokenRequest
{
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("password")]
    public string Password { get; set; }
}
