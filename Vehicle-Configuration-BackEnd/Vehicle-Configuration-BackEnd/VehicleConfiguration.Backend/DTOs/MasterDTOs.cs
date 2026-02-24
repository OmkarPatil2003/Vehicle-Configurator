namespace VehicleConfiguration.Backend.DTOs;

public class SegmentDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class ManufacturerDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class ModelDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public string? ImagePath { get; set; }
    public int MinQty { get; set; }
}
