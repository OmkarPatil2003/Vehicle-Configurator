using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleConfiguration.Backend.Models;

[Table("user")]
public class User
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("add1")]
    public string Add1 { get; set; } = string.Empty;

    [Column("add2")]
    public string? Add2 { get; set; }

    [Column("auth_name")]
    public string AuthName { get; set; } = string.Empty;

    [Column("auth_tel")]
    public string AuthTel { get; set; } = string.Empty;

    [Column("cell")]
    public string? Cell { get; set; }

    [Column("city")]
    public string City { get; set; } = string.Empty;

    [Column("company_name")]
    public string CompanyName { get; set; } = string.Empty;

    [Column("company_st_no")]
    public string CompanyStNo { get; set; } = string.Empty;

    [Column("company_vat_no")]
    public string CompanyVatNo { get; set; } = string.Empty;

    [Column("designation")]
    public string Designation { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("fax")]
    public string? Fax { get; set; }

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("pin")]
    public string? Pin { get; set; }

    [Column("state")]
    public string State { get; set; } = string.Empty;

    [Column("tax_pan")]
    public string? TaxPan { get; set; }

    [Column("tel")]
    public string? Tel { get; set; }

    [Column("holding_type")]
    public string? HoldingType { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("role")]
    public string? Role { get; set; }

    [Column("username")]
    public string? Username { get; set; }

    [Column("registration_no")]
    public string? RegistrationNo { get; set; }

    [Column("failed_attempts")]
    public int FailedAttempts { get; set; }

    [Column("is_blocked")]
    public bool IsBlocked { get; set; }
}
