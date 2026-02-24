using Microsoft.EntityFrameworkCore;
using VehicleConfiguration.Backend.Models;

namespace VehicleConfiguration.Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Segment> Segments { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<AlternateComponentMaster> AlternateComponentMasters { get; set; }
    public DbSet<SgMfgMaster> SgMfgMasters { get; set; }
    public DbSet<VehicleDefaultConfig> VehicleDefaultConfigs { get; set; }
    public DbSet<VehicleDetail> VehicleDetails { get; set; }
    public DbSet<InvoiceHeader> InvoiceHeaders { get; set; }
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
