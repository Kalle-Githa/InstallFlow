namespace InstallFlow.Data.DTO.Customers
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Company { get; set; } = null!;
        public string? Person { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? OrganizationNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


    }
}
