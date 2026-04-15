namespace InstallFlow.Data.DTO
{
    public class CreateCartDto
    {
        public int UserId { get; set; }
        public int? AssignmentId { get; set; }
        public int? JobId { get; set; }
    }
}