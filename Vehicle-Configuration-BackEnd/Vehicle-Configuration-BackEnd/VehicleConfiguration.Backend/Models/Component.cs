using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleConfiguration.Backend.Models;

[Table("component")]
public class Component
{
    [Key]
    [Column("comp_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CompId { get; set; }

    [Column("comp_name")]
    public string? CompName { get; set; }

    [Column("comp_type")]
    public string? Type { get; set; }

    [Column("price")]
    public double Price { get; set; }

    public virtual ICollection<AlternateComponentMaster> AlternateComponentMasters { get; set; } = new List<AlternateComponentMaster>();
    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
    public virtual ICollection<VehicleDetail> VehicleDetails { get; set; } = new List<VehicleDetail>();
}
