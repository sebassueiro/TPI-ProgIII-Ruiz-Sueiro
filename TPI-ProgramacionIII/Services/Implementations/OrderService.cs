using Microsoft.EntityFrameworkCore;
using TPI_ProgramacionIII.Data.Entities;
using TPI_ProgramacionIII.DBContexts;
using TPI_ProgramacionIII.Services.Interfaces;

namespace TPI_ProgramacionIII.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ECommerceContext _context;

        public OrderService(ECommerceContext context)
        {
            _context = context;
        }

        public List<Order> GetAllByClient(int clientId) //todas por cliente
        {
            return _context.Orders
                .Include(so => so.Client)
                .Include(so => so.LinesOfOrder)
                .ThenInclude(so => so.Product)
                .Where(r => r.ClientId == clientId)
                .ToList();
        }

        public Order? GetOne(int Id)
        {
            return _context.Orders
                .Include(r => r.Client)
                .Include(r => r.LinesOfOrder)
                .ThenInclude(so => so.Product)
                .SingleOrDefault(x => x.Id == Id);
        }

        public List<Order> GetAllByDate(DateTime date)
        {
            return _context.Orders
                .Include(r => r.Client)
                .Include(r => r.LinesOfOrder)
                .ThenInclude(so => so.Product)
                .Where(r => r.Date.Date == date.Date)
                .ToList();
        }

        public Order CreateSaleOrder(Order order)
        {
            _context.Add(order);
            _context.SaveChanges();
            return order;
        }

        public Order UpdateSaleOrder(Order order)
        {
            _context.Update(order);
            _context.SaveChanges();
            return order;
        }

        public void DeleteSaleOrder(int id)
        {
            var saleOrderToDelete = _context.Orders.SingleOrDefault(p => p.Id == id);

            if (saleOrderToDelete != null)
            {
                _context.Orders.Remove(saleOrderToDelete);
                _context.SaveChanges();
            }

        }
    }
}
