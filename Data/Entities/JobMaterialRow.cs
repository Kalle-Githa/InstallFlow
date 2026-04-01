namespace InstallFlow.Data.Entities
{
    public class JobMaterialRow
    {
        public int Id { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; } = null!;

        public string Name { get; set; } = null!;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int SortOrder { get; set; }
        public bool IsExtraMaterial { get; set; }

        //public int ProductId { get; set; }
        //public Product Product { get; set; }

    }
}
