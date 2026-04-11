namespace InstallFlow.Data.DTO;

using System.ComponentModel.DataAnnotations;

public class UpdateAssignmentDto
{
    [StringLength(200, MinimumLength = 1)]
    public string? Name { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(300)]
    public string? Address { get; set; }

    public string? Status { get; set; }  // "Draft", "InProgress", etc.
}