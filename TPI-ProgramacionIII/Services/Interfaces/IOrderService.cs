using TPI_ProgramacionIII.Data.Entities;

namespace TPI_ProgramacionIII.Services.Interfaces
{
    public interface IOrderService
    {
        List<Order> GetAllByClient(int id);
        List<Order> GetAllByDate(DateTime date);
        Order? GetOne(int Id);
        Order CreateSaleOrder(Order order);
        Order UpdateSaleOrder(Order order);
        void DeleteSaleOrder(int id);
    }
}
