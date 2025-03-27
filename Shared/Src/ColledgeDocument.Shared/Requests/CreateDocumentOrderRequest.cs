namespace ColledgeDocument.Shared.Requests;

public class CreateDocumentOrderRequest
{
    [JsonPropertyName("document_type_id")]
    public int DocumentTypeId { get; set; }
    [JsonPropertyName("departament_id")]
    public int DepartamnetId { get; set; }
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}
