namespace VehicleConfiguration.Backend.DTOs;

public class AlternateComponentDTO
{
    public int CompId { get; set; }
    public int AltCompId { get; set; }
}

public class AlternateComponentSaveDTO
{
    public int ModelId { get; set; }
    public List<AlternateComponentDTO> Components { get; set; } = new();
}
