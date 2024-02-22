namespace TPI_ProgramacionIII.Data.Entities
{
    public class Client : User
    {
        public string Address { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
