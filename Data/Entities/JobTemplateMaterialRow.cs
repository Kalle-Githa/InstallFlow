namespace InstallFlow.Data.Entities
{
    public class JobTemplateMaterialRow
    {
        public int Id { get; set; }

        public int JobTemplateId { get; set; }
        public JobTemplate JobTemplate { get; set; } = null!;

        public string Name { get; set; } = null!;
        public decimal DefaultQuantity { get; set; }
        public string Unit { get; set; } = null!;
        public decimal DefaultUnitPrice { get; set; }
        public int SortOrder { get; set; }
        public bool IsOptional { get; set; }
        public bool IsExtraMaterial { get; set; }

        //public int ProductId { get; set; }
        //public Product Product { get; set; }








    }
}
