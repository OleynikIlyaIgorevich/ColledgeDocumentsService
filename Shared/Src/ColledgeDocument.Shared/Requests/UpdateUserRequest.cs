namespace ColledgeDocument.Shared.Requests;

public class UpdateUserRequest
{
    [JsonPropertyName("lastname")]
    public string LastName { get; set; }
    [JsonPropertyName("firstname")]
    public string FirstName { get; set; }
    [JsonPropertyName("middlename")]
    public string? MiddleName { get; set; }
    [JsonPropertyName("phone")]
    public string Phone { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
    [JsonPropertyName("is_change_password")]
    public bool IsChangePassword { get; set; }
    [JsonPropertyName("password")]
    public string? Password { get; set; }
    [JsonPropertyName("role_id")]
    public int RoleId { get; set; }
}
