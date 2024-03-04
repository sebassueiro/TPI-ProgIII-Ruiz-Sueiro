using System.ComponentModel.DataAnnotations;

namespace TPI_ProgramacionIII.Models
{
    public class ProductsDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
