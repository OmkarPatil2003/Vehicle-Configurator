using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleConfiguration.Backend.Models;

[Table("invoice_header")]
public class InvoiceHeader
{
    [Key]
    [Column("inv_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    [Column("model_id")]
    public int ModelId { get; set; }

    [ForeignKey("ModelId")]
    public virtual Model Model { get; set; }

    [Column("qty")]
    public int Qty { get; set; }

    [Column("base_amt")]
    public double BaseAmt { get; set; }

    [Column("tax")]
    public double Tax { get; set; }

    [Column("total_amt")]
    public double TotalAmt { get; set; }

    [Column("inv_date")]
    public DateOnly? InvDate { get; set; }
    // Note: Java used LocalDate, C# DateOnly is good for MySQL date.

    [Column("status")]
    public string Status { get; set; } 
    // Java used EnumType.STRING. We can map string to enum in code or just use string. 
    // EF generic conversion is easy. I'll use string property backing. 
    // Or I can use the enum directly if I configure conversion.
    // For simplicity, I'll use string property here to match database column exactly and parse it if needed, 
    // OR just use enum with string conversion in OnModelCreating.
    // Let's use string to be safe on reading, or Enum with string converter.
    // I will use string here to match Java's @Enumerated(EnumType.STRING) which stores string in DB. 
    // But better to use Enum in C# model and configure conversion in DbContext.
    // I will stick to string here for 100% safety against parsing errors for now, or just valid Enum. 
    // I'll use InvoiceStatus enum property but need conversion.
    // Let's stick with string for direct mapping simplicity unless I add the conversion.
    // Java: @Enumerated(EnumType.STRING) private InvoiceStatus status;
    // C#: public InvoiceStatus Status { get; set; } => requires .HasConversion<string>();
    
    [Column("customer_detail")]
    public string? CustomerDetail { get; set; }
}
