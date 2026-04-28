// CreateCustomerDto.cs
using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Customers;

public class CreateCustomerDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Company { get; set; } = null!;

    [StringLength(200)]
    public string? Person { get; set; }

    [StringLength(200)]
    [EmailAddress(ErrorMessage = "Ogiltig e-postadress.")]
    public string? Email { get; set; }

    [StringLength(50)]
    [Phone(ErrorMessage = "Ogiltigt telefonnummer.")]
    public string? Phone { get; set; }

    [StringLength(20)]
    public string? OrganizationNumber { get; set; }
}