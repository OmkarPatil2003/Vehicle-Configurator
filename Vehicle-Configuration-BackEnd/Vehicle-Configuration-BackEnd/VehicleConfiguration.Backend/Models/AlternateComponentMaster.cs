using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleConfiguration.Backend.Models;

[Table("alternate_component_master")]
public class AlternateComponentMaster
{
    [Key]
    [Column("alt_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("delta_price")]
    public double? DeltaPrice { get; set; }

    [Column("alt_comp_id")]
    public int? AltCompId { get; set; }

    [ForeignKey("AltCompId")]
    public virtual Component? AltComp { get; set; }

    [Column("comp_id")]
    public int? CompId { get; set; }

    [ForeignKey("CompId")]
    [InverseProperty("AlternateComponentMasters")]
    public virtual Component? Comp { get; set; }

    [Column("model_id")]
    public int? ModelId { get; set; }

    [ForeignKey("ModelId")]
    public virtual Model? Model { get; set; }
}
