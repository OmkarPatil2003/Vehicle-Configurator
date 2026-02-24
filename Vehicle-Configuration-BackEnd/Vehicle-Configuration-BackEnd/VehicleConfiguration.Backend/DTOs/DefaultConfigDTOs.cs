using System.Text.Json.Serialization;

namespace VehicleConfiguration.Backend.DTOs;

/**
 * DefaultConfigDTOs.cs
 * Purpose: Defines the Data Transfer Objects (DTOs) used for the Default Configuration feature.
 * 
 * Classes:
 * - DefaultConfigurationDTO: Lightweight object (Id, Name) for simple lists.
 * - ComponentDTO: Represents a single vehicle component.
 * - DefaultConfigResponseDTO: Comprehensive object containing Model details, pricing, and the list of default components.
 * 
 * Note: [JsonPropertyName] attributes are used to ensure strict camelCase JSON output, matching the frontend's expectations.
 */

public class DefaultConfigurationDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    public DefaultConfigurationDTO() {}
    public DefaultConfigurationDTO(int id, string? name)
    {
        Id = id;
        Name = name;
    }
}

public class ComponentDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    public ComponentDTO() {}
    public ComponentDTO(int id, string? name)
    {
        Id = id;
        Name = name;
    }
}

public class DefaultConfigResponseDTO
{
    [JsonPropertyName("modelId")]
    public int ModelId { get; set; }

    [JsonPropertyName("modelName")]
    public string? ModelName { get; set; }

    [JsonPropertyName("segmentName")]
    public string? SegmentName { get; set; }

    [JsonPropertyName("manufacturerName")]
    public string? ManufacturerName { get; set; }

    [JsonPropertyName("basePrice")]
    public double BasePrice { get; set; }

    [JsonPropertyName("minQuantity")]
    public int MinQuantity { get; set; }

    [JsonPropertyName("totalPrice")]
    public double TotalPrice { get; set; }

    [JsonPropertyName("defaultComponents")]
    public List<ComponentDTO>? DefaultComponents { get; set; }

    public DefaultConfigResponseDTO() {}
    public DefaultConfigResponseDTO(int modelId, string? modelName, string? segmentName, string? manufacturerName, double basePrice, int minQuantity, double totalPrice, List<ComponentDTO>? defaultComponents)
    {
        ModelId = modelId;
        ModelName = modelName;
        SegmentName = segmentName;
        ManufacturerName = manufacturerName;
        BasePrice = basePrice;
        MinQuantity = minQuantity;
        TotalPrice = totalPrice;
        DefaultComponents = defaultComponents;
    }
}
