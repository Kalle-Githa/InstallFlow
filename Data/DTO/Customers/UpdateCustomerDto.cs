using System.ComponentModel.DataAnnotations;

namespace InstallFlow.Data.DTO.Customers;

public class UpdateCustomerDto
{
    public string? Company { get; set; }
    public string? Person { get; set; }

    [EmailAddress(ErrorMessage = "Ogiltig e-postadress.")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Ogiltigt telefonnummer.")]
    public string? Phone { get; set; }

    public string? OrganizationNumber { get; set; }
}