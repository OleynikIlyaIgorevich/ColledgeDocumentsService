namespace ColledgeDocument.Shared.Requests;

public class UpdatePasswordRequest
{
    [JsonPropertyName("current_password")]
    public string CurrentPassword { get; set; }
    [JsonPropertyName("new_password")]
    public string NewPassword { get; set; }
    [JsonPropertyName("repeat_new_password")]
    public string RepeatNewPassword { get; set; }
}
