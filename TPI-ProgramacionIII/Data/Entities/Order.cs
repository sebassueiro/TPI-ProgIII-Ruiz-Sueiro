using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPI_ProgramacionIII.Data.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [ForeignKey("ClientId")]
        public int ClientId { get; set; } // Clave foránea para la relación con Cliente
        public Client Client { get; set; }
        public ICollection<LineOfOrder> LinesOfOrder { get; set; } = new List<LineOfOrder>();
    }
}
