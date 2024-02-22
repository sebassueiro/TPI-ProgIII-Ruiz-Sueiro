namespace TPI_ProgramacionIII.Models
{
    public class LineOfOrderDto
    {
        public int Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductId { get; set; }
        public int SaleOrderId { get; set; }
    }
}
