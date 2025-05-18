namespace ColledgeDocument.Shared.Requests;

public class UpdateDocumentOrderRequest
{
    [JsonPropertyName("document_type_id")]
    public int DocumentTypeId { get; set; }
    [JsonPropertyName("departament_id")]
    public int DepartmentId { get; set; }
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
    [JsonPropertyName("order_status_id")]
    public int OrderStatusId { get; set; }
}
