using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleConfiguration.Backend.DTOs;
using VehicleConfiguration.Backend.Models;
using VehicleConfiguration.Backend.Repositories;

namespace VehicleConfiguration.Backend.Services;

/**
 * VehicleService.cs
 * Purpose: Contains the core business logic for vehicle configuration.
 * 
 * Responsibilities:
 * - Arranges data retrieval for Default Configuration (handling Raw SQL fallback and DTO mapping).
 * - Manages Configurable Components options (grouping by component name).
 * - Handles the logic for saving Alternate Components (admin feature).
 * - Bridges the Controller layer and the Repository layer.
 */
public class VehicleService
{
    // ... fields ... (Assume matching StartLine context)
    private readonly IVehicleDetailRepository _vehicleRepo;
    private readonly IModelRepository _modelRepo;
    private readonly IComponentRepository _compRepo;
    private readonly IAlternateComponentRepository _altRepo;
    private readonly IDefaultConfigRepository _defaultRepo;
    private readonly ILogger<VehicleService> _logger;

    public VehicleService(
        IVehicleDetailRepository vehicleRepo,
        IModelRepository modelRepo,
        IComponentRepository compRepo,
        IAlternateComponentRepository altRepo,
        IDefaultConfigRepository defaultRepo,
        ILogger<VehicleService> logger)
    {
        _vehicleRepo = vehicleRepo;
        _modelRepo = modelRepo;
        _compRepo = compRepo;
        _altRepo = altRepo;
        _defaultRepo = defaultRepo;
        _logger = logger;
    }
    
    // ... Methods ...



    public async Task<List<ComponentDropdownDto>> GetConfigurableComponentsAsync(int modelId, string compType)
    {
        var details = await _vehicleRepo.FindConfigurableComponentsAsync(modelId, compType);

        // Group by Component Name
        var groupedMap = new Dictionary<string, List<OptionDto>>();

        foreach (var vd in details)
        {
            if (vd.Comp == null) continue;

            string componentName = vd.Comp.CompName ?? "Unknown";
            int compId = vd.Comp.CompId;
            string subType = vd.Comp.Type ?? "";
            double price = vd.Comp.Price;

            var option = new OptionDto(compId, subType, price);

            if (!groupedMap.ContainsKey(componentName))
            {
                groupedMap[componentName] = new List<OptionDto>();
            }
            groupedMap[componentName].Add(option);
        }

        // Convert to DTO List
        var response = new List<ComponentDropdownDto>();
        foreach (var kvp in groupedMap)
        {
            response.Add(new ComponentDropdownDto(kvp.Key, kvp.Value));
        }

        return response;
    }

    public async Task SaveAlternateComponentsAsync(AlternateComponentSaveDTO dto)
    {
        var model = await _modelRepo.GetByIdAsync(dto.ModelId);
        if (model == null) throw new Exception("Model not found");

        var components = await _compRepo.GetAllAsync(); 

        foreach (var item in dto.Components)
        {
             var componentsList = components;
             
             var original = componentsList.FirstOrDefault(c => c.CompId == item.CompId);
             var alternate = componentsList.FirstOrDefault(c => c.CompId == item.AltCompId);

             if (original == null) throw new Exception("Original component not found");
             if (alternate == null) throw new Exception("Alternate component not found");

             if (original.CompName != alternate.CompName) throw new Exception("Invalid component replacement");

             double deltaPrice = alternate.Price - original.Price;
             
             var existing = await _altRepo.GetByModelIdAndCompIdAsync(dto.ModelId, original.CompId);
             
             var acm = existing.FirstOrDefault() ?? new AlternateComponentMaster();
             
             acm.ModelId = model.Id;
             acm.CompId = original.CompId;
             acm.AltCompId = alternate.CompId;
             acm.DeltaPrice = deltaPrice;
             
             await _altRepo.SaveAsync(acm);
        }
    }

    public async Task<DefaultConfigResponseDTO> GetDefaultConfigurationAsync(int modelId, int quantity)
    {
        // Use GetByIdWithDetailsAsync to ensure Seg and Mfg are loaded
        var model = await _modelRepo.GetByIdWithDetailsAsync(modelId);
        if (model == null) throw new Exception("Invalid model");

        // Use Raw SQL method to guarantee component fetching without EF loading issues
        var defaultComps = await _vehicleRepo.GetDefaultComponentsRawAsync(modelId);
        
        var compDtos = defaultComps.Select(v => {
             return new ComponentDTO(v.CompId, v.CompName ?? "Unknown");
        }).ToList();

        double unitPrice = model.Price;
        double totalPrice = unitPrice * quantity;

        return new DefaultConfigResponseDTO(
            model.Id,
            model.ModelName,
            model.Seg?.SegName ?? "Unknown", 
            model.Mfg?.MfgName ?? "Unknown",
            unitPrice,
            model.MinQty,
            totalPrice,
            compDtos
        );
    }
    
    public async Task<List<DefaultConfigurationDTO>> GetDefaultConfigurationSimpleAsync(int modelId)
    {
         var list = await _defaultRepo.GetByModelIdAsync(modelId);
         // Map actual Component Name to the Name field so frontend displays it correctly
         return list.Select(v => new DefaultConfigurationDTO(v.Comp!.CompId, v.Comp.CompName)).ToList();
    }

    public async Task<List<string?>> IsConfigurableAsync(int modelId, string compType)
    {
        return await _vehicleRepo.FindConfigurableVehicleDetailsAsync(modelId, compType);
    }
}
