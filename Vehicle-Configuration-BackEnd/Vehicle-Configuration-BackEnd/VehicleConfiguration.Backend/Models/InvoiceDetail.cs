using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleConfiguration.Backend.Models;

[Table("invoice_detail")]
public class InvoiceDetail
{
    [Key]
    [Column("inv_dtl_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("comp_id")]
    public int? CompId { get; set; }

    [ForeignKey("CompId")]
    public virtual Component? Comp { get; set; }

    [Column("inv_id")]
    public int? InvId { get; set; }

    [ForeignKey("InvId")]
    public virtual InvoiceHeader? Inv { get; set; }

    [Column("comp_price")]
    public double CompPrice { get; set; }
}
