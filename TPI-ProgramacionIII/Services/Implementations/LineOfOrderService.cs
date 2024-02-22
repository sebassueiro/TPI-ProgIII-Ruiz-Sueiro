using Microsoft.EntityFrameworkCore;
using TPI_ProgramacionIII.Data.Entities;
using TPI_ProgramacionIII.DBContexts;
using TPI_ProgramacionIII.Services.Interfaces;

namespace TPI_ProgramacionIII.Services.Implementations
{
    public class LineOfOrderService : ILineOfOrderService
    {
        private readonly ECommerceContext _context;
        public LineOfOrderService(ECommerceContext context)
        {
            _context = context;
        }

        public List<LineOfOrder> GetAllBySaleOrder(int orderId)
        {
            return _context.LinesOfOrder
                .Include(sol => sol.Product)
                .Include(sol => sol.SaleOrder)
                .ThenInclude(so => so.Client)
                .Where(sol => sol.SaleOrderId == orderId)
                .ToList();
        }

        public List<LineOfOrder> GetAllByProduct(int productId)
        {
            return _context.LinesOfOrder
                .Include(sol => sol.Product)
                .Where(sol => sol.ProductId == productId)
                .Include(sol => sol.SaleOrder)
                .ThenInclude(so => so.Client)
                .ToList();
        }

        public LineOfOrder? GetOne(int Id)
        {
            return _context.LinesOfOrder
                .Include(sol => sol.Product)
                .Include(sol => sol.SaleOrder)
                .ThenInclude(so => so.Client)
                .SingleOrDefault(x => x.Id == Id);
        }

        public LineOfOrder CreateSaleOrderLine(LineOfOrder lineOfOrder)
        {
            _context.Add(lineOfOrder);
            _context.SaveChanges();
            return lineOfOrder;
        }

        public LineOfOrder UpdateSaleOrderLine(LineOfOrder lineOfOrder)
        {
            _context.Update(lineOfOrder);
            _context.SaveChanges();
            return lineOfOrder;
        }

        public void DeleteSaleOrderLine(int id)
        {
            var solToDelete = _context.LinesOfOrder.SingleOrDefault(p => p.Id == id);

            if (solToDelete != null)
            {
                _context.LinesOfOrder.Remove(solToDelete);
                _context.SaveChanges();
            }
        }
    }
}
