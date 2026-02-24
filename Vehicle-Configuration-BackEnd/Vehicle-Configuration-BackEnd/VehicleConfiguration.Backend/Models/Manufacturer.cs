using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleConfiguration.Backend.Models;

[Table("manufacturer")]
public class Manufacturer
{
    [Key]
    [Column("mfg_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("mfg_name")]
    public string? MfgName { get; set; }
}
