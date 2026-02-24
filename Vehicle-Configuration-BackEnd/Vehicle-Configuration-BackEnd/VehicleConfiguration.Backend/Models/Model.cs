using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleConfiguration.Backend.Models;

[Table("model")]
public class Model
{
    [Key]
    [Column("model_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("img_path")]
    public string? ImgPath { get; set; }

    [Column("min_qty")]
    public int MinQty { get; set; }

    [Column("model_name")]
    public string? ModelName { get; set; }

    [Column("mfg_id")]
    public int? MfgId { get; set; }

    [ForeignKey("MfgId")]
    public virtual Manufacturer? Mfg { get; set; }

    [Column("seg_id")]
    public int? SegId { get; set; }

    [ForeignKey("SegId")]
    public virtual Segment? Seg { get; set; }

    [Column("price")]
    public double Price { get; set; }
}
