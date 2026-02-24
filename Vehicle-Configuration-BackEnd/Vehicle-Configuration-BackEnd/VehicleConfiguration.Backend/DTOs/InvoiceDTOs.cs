namespace VehicleConfiguration.Backend.DTOs;

using System.Text.Json.Serialization;

public class InvoiceRequestDTO
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("modelId")]
    public int ModelId { get; set; }

    [JsonPropertyName("qty")]
    public int Qty { get; set; }

    [JsonPropertyName("customerDetail")]
    public string? CustomerDetail { get; set; }

    [JsonPropertyName("alternates")]
    public List<int>? Alternates { get; set; }
}
