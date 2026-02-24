using Microsoft.EntityFrameworkCore;
using VehicleConfiguration.Backend.Data;
using VehicleConfiguration.Backend.Models;

namespace VehicleConfiguration.Backend.Repositories
{
    public interface ISegmentRepository { Task<List<Segment>> GetAllAsync(); }
    public interface IManufacturerRepository { Task<List<Manufacturer>> GetAllAsync(); Task<List<Manufacturer>> GetBySegmentIdAsync(int segId); }
    public interface IModelRepository { Task<List<Model>> GetAllAsync(); Task<List<Model>> GetBySegAndMfgAsync(int segId, int mfgId); Task<Model?> GetByIdAsync(int id); Task<Model?> GetByIdWithDetailsAsync(int id); }
    public interface IComponentRepository { Task<List<Component>> GetAllAsync(); }
    public interface IDefaultConfigRepository { Task<List<VehicleDefaultConfig>> GetByModelIdAsync(int modelId); }
    public interface IAlternateComponentRepository { Task<List<AlternateComponentMaster>> GetByModelIdAndCompIdAsync(int modelId, int compId); Task SaveAsync(AlternateComponentMaster entity); }
    public interface ISgMfgMasterRepository { Task<List<SgMfgMaster>> GetBySegmentIdAsync(int segId); }
}

namespace VehicleConfiguration.Backend.Repositories.Impl
{
    public class SegmentRepository : ISegmentRepository
    {
        private readonly ApplicationDbContext _context;
        public SegmentRepository(ApplicationDbContext context) { _context = context; }
        public async Task<List<Segment>> GetAllAsync() => await _context.Segments.ToListAsync();
    }

    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ApplicationDbContext _context;
        public ManufacturerRepository(ApplicationDbContext context) { _context = context; }
        public async Task<List<Manufacturer>> GetAllAsync() => await _context.Manufacturers.ToListAsync();
        public async Task<List<Manufacturer>> GetBySegmentIdAsync(int segId)
        {
            // Join via SgMfgMaster
            return await _context.SgMfgMasters
                .Where(x => x.SegId == segId)
                .Include(x => x.Mfg)
                .Select(x => x.Mfg)
                .ToListAsync();
        }
    }

    public class ModelRepository : IModelRepository
    {
        private readonly ApplicationDbContext _context;
        public ModelRepository(ApplicationDbContext context) { _context = context; }
        public async Task<List<Model>> GetAllAsync() => await _context.Models.ToListAsync();
        public async Task<List<Model>> GetBySegAndMfgAsync(int segId, int mfgId) => 
            await _context.Models.Where(m => m.SegId == segId && m.MfgId == mfgId).ToListAsync();
        public async Task<Model?> GetByIdAsync(int id) => await _context.Models.FindAsync(id);
        public async Task<Model?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Models
                .Include(m => m.Seg)
                .Include(m => m.Mfg)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
    
    public class ComponentRepository : IComponentRepository
    {
        private readonly ApplicationDbContext _context;
        public ComponentRepository(ApplicationDbContext context) { _context = context; }
        public async Task<List<Component>> GetAllAsync() => await _context.Components.ToListAsync();
    }

    public class DefaultConfigRepository : IDefaultConfigRepository
    {
        private readonly ApplicationDbContext _context;
        public DefaultConfigRepository(ApplicationDbContext context) { _context = context; }
        public async Task<List<VehicleDefaultConfig>> GetByModelIdAsync(int modelId) => 
            await _context.VehicleDefaultConfigs
                .Where(c => c.ModelId == modelId)
                .Include(c => c.Comp)
                .ToListAsync();
    }

    public class AlternateComponentRepository : IAlternateComponentRepository
    {
        private readonly ApplicationDbContext _context;
        public AlternateComponentRepository(ApplicationDbContext context) { _context = context; }
        public async Task<List<AlternateComponentMaster>> GetByModelIdAndCompIdAsync(int modelId, int compId) => 
            await _context.AlternateComponentMasters
                .Where(a => a.ModelId == modelId && a.CompId == compId)
                .Include(a => a.AltComp)
                .ToListAsync();
        
        public async Task SaveAsync(AlternateComponentMaster entity)
        {
            if (entity.Id == 0)
            {
                _context.AlternateComponentMasters.Add(entity);
            }
            else
            {
                _context.AlternateComponentMasters.Update(entity);
            }
           await _context.SaveChangesAsync();
        }
    }
}
