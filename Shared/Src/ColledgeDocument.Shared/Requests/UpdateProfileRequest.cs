namespace ColledgeDocument.Shared.Requests;

public class UpdateProfileRequest
{
    [JsonPropertyName("lastname")]
    public string LastName { get; set; }
    [JsonPropertyName("firstname")]
    public string FirstName { get; set; }
    [JsonPropertyName("middlename")]
    public string MiddleName { get; set; }
    [JsonPropertyName("phone")]
    public string Phone { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
}
