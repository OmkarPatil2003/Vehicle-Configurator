using Microsoft.EntityFrameworkCore;
using VehicleConfiguration.Backend.Data;
using VehicleConfiguration.Backend.Models;


namespace VehicleConfiguration.Backend.Repositories
{
    /**
     * VehicleDetailRepository.cs
     * Purpose: Interface and Implementation for accessing 'vehicle_detail' data.
     * 
     * Key Methods:
     * - FindDefaultComponentsAsync: Uses EF Core .Include() to fetch related Component data.
     * - GetDefaultComponentsRawAsync: Uses Raw SQL as a robust fallback to fetch component details, bypassing potential ORM mapping complexities.
     * - FindConfigurableComponentsAsync: Fetches all available options for a given model and component type.
     */
    public interface IVehicleDetailRepository
    {
        Task<List<VehicleDetail>> FindConfigurableComponentsAsync(int modelId, string compType);
        Task<List<VehicleDetail>> FindDefaultComponentsAsync(int modelId);
        Task<List<string?>> FindConfigurableVehicleDetailsAsync(int modelId, string compType);
        Task<List<Component>> GetDefaultComponentsExplicitAsync(int modelId);
        Task<List<Component>> GetDefaultComponentsRawAsync(int modelId);
    }
}

namespace VehicleConfiguration.Backend.Repositories.Impl
{
    public class VehicleDetailRepository : IVehicleDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleDetail>> FindConfigurableComponentsAsync(int modelId, string compType)
        {
            return await _context.VehicleDetails
                .Include(v => v.Comp)
                .Where(v => v.ModelId == modelId && v.CompType == compType && v.IsConfig == "Y")
                .ToListAsync();
        }

        public async Task<List<VehicleDetail>> FindDefaultComponentsAsync(int modelId)
        {
            // where vd.model.id = :modelId and vd.isConfig = 'Y'
            return await _context.VehicleDetails
                .Include(v => v.Comp)
                .Where(v => v.ModelId == modelId && v.IsConfig == "Y")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Component>> GetDefaultComponentsRawAsync(int modelId)
        {
            // Fallback: Raw SQL to ensure ID and Name are retrieved if mapping fails
            // Force case-insensitive mapping if needed
            var sql = @"
                SELECT c.comp_id AS CompId, c.comp_name AS CompName, c.comp_type AS Type, c.price AS Price
                FROM vehicle_detail vd
                JOIN component c ON vd.comp_id = c.comp_id
                WHERE vd.model_id = {0} AND vd.is_config = 'Y'";

            // Using FromSqlRaw to map directly to Component entity
            return await _context.Components
                .FromSqlRaw(sql, modelId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<string?>> FindConfigurableVehicleDetailsAsync(int modelId, string compType)
        {
             // SELECT v.comp.compName FROM VehicleDetail v WHERE ...
             return await _context.VehicleDetails
                .Include(v => v.Comp)
                .Where(v => v.ModelId == modelId && v.CompType == compType && v.IsConfig == "Y")
                .Select(v => v.Comp!.CompName)
                .ToListAsync();
        }

        public async Task<List<Component>> GetDefaultComponentsExplicitAsync(int modelId)
        {
            // Raw SQL to absolutely guarantee we get the data from the DB, bypassing any EF Core Join weirdness
            // Using aliases to ensure mapping matches C# property names case-sensitively if needed by provider
            var sql = @"
                SELECT 
                    c.comp_id, 
                    c.comp_name, 
                    c.comp_type, 
                    c.price
                FROM component c
                INNER JOIN vehicle_detail vd ON c.comp_id = vd.comp_id
                WHERE vd.model_id = {0} AND vd.is_config = 'Y'";

            return await _context.Components
                .FromSqlRaw(sql, modelId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
