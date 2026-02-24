namespace VehicleConfiguration.Backend.DTOs;

public class OptionDto
{
    public int CompId { get; set; }
    public string? SubType { get; set; }
    public double Price { get; set; }

    public OptionDto(int compId, string? subType, double price)
    {
        CompId = compId;
        SubType = subType;
        Price = price;
    }
}

public class ComponentDropdownDto
{
    public string? ComponentName { get; set; }
    public List<OptionDto> Options { get; set; } = new List<OptionDto>();

    public ComponentDropdownDto(string? componentName, List<OptionDto> options)
    {
        ComponentName = componentName;
        Options = options;
    }
}
