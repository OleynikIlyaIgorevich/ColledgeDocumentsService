namespace ColledgeDocument.Shared.Requests;

public class CreateDocumentTypeRequest
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
}
