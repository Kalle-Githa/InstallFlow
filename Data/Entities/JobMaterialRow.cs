using InstallFlow.Data.Enums;

namespace InstallFlow.Data.Entities
{
    public class JobMaterialRow
    {
        public int Id { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; } = null!;

        public string Name { get; set; } = null!;
        public decimal Quantity { get; set; }
        public UnitType Unit { get; set; } = UnitType.Piece;
        public decimal UnitPrice { get; set; }
       
        public bool IsExtraMaterial { get; set; }

        //public int ProductId { get; set; }
        //public Product Product { get; set; }

    }
}
