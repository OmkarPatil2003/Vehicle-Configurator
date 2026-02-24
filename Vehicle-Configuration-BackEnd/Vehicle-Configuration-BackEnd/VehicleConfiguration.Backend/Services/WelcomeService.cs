using VehicleConfiguration.Backend.DTOs;
using VehicleConfiguration.Backend.Repositories;

namespace VehicleConfiguration.Backend.Services;

public class WelcomeService
{
    private readonly ISegmentRepository _segmentRepo;
    private readonly IManufacturerRepository _mfgRepo;
    private readonly IModelRepository _modelRepo;

    public WelcomeService(ISegmentRepository segmentRepo, IManufacturerRepository mfgRepo, IModelRepository modelRepo)
    {
        _segmentRepo = segmentRepo;
        _mfgRepo = mfgRepo;
        _modelRepo = modelRepo;
    }

    public async Task<List<SegmentDTO>> GetAllSegmentsAsync()
    {
        var list = await _segmentRepo.GetAllAsync();
        return list.Select(s => new SegmentDTO { Id = s.Id, Name = s.SegName }).ToList();
    }

    public async Task<List<ManufacturerDTO>> GetManufacturersBySegmentAsync(int segId)
    {
        var list = await _mfgRepo.GetBySegmentIdAsync(segId);
        return list.Select(m => new ManufacturerDTO { Id = m.Id, Name = m.MfgName }).ToList();
    }

    public async Task<List<ModelDTO>> GetModelsAsync(int segId, int mfgId)
    {
        var list = await _modelRepo.GetBySegAndMfgAsync(segId, mfgId);
        return list.Select(m => new ModelDTO 
        { 
            Id = m.Id, 
            Name = m.ModelName,
            Price = m.Price,
            ImagePath = m.ImgPath,
            MinQty = m.MinQty
        }).ToList();
    }
}
