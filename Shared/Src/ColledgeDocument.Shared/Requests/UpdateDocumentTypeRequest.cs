namespace ColledgeDocument.Shared.Requests;

public class UpdateDocumentTypeRequest
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
}
