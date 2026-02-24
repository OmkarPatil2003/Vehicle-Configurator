using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleConfiguration.Backend.Models;

[Table("sg_mfg_master")]
public class SgMfgMaster
{
    [Key]
    [Column("sgmf_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("mfg_id")]
    public int MfgId { get; set; }
    
    [ForeignKey("MfgId")]
    public virtual Manufacturer Mfg { get; set; }

    [Column("seg_id")]
    public int SegId { get; set; }
    
    [ForeignKey("SegId")]
    public virtual Segment Seg { get; set; }
}
