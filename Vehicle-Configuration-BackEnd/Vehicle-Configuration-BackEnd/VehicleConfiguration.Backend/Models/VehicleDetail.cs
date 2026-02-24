using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleConfiguration.Backend.Models;

[Table("vehicle_detail")]
public class VehicleDetail
{
    [Key]
    [Column("config_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ConfigId { get; set; }

    [Column("comp_type")]
    public string? CompType { get; set; } // S / I / E / C

    [Column("is_config")]
    public string? IsConfig { get; set; } // Y / N

    [Column("comp_id")]
    public int? CompId { get; set; }

    [ForeignKey("CompId")]
    public virtual Component? Comp { get; set; }

    [Column("model_id")]
    public int? ModelId { get; set; }

    [ForeignKey("ModelId")]
    public virtual Model? Model { get; set; }
}
