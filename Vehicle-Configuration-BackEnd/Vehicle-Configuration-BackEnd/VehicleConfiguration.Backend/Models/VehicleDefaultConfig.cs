using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleConfiguration.Backend.Models;

[Table("vehicle_default_config")]
public class VehicleDefaultConfig
{
    [Key]
    [Column("config_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("model_id")]
    public int ModelId { get; set; }

    [ForeignKey("ModelId")]
    public virtual Model Model { get; set; }

    [Column("comp_id")]
    public int CompId { get; set; }

    [ForeignKey("CompId")]
    public virtual Component Comp { get; set; }
}
