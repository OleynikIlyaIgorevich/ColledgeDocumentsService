namespace ColledgeDocument.Shared.Requests;

public class UpdateProfileRequest
{
    [JsonPropertyName("lastname")]
    public string Lastname { get; set; }
    [JsonPropertyName("firstname")]
    public string Firstname { get; set; }
    [JsonPropertyName("middlename")]
    public string Middlename { get; set; }
    [JsonPropertyName("phone")]
    public string Phone { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
}
