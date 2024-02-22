using Microsoft.EntityFrameworkCore;
using TPI_ProgramacionIII.Data.Entities;
using TPI_ProgramacionIII.DBContexts;
using TPI_ProgramacionIII.Services.Interfaces;

namespace TPI_ProgramacionIII.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ECommerceContext _context;

        public ProductService(ECommerceContext context)
        {
            _context = context;
        }

        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public Product? GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public Product? GetProductByName(string name)
        {
            return _context.Products.FirstOrDefault(p => p.Name == name);
        }

        public int CreateProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            return (product.Id);
        }

        public void DeleteProduct(int id)
        {
            var productToDelete = _context.Products.SingleOrDefault(p => p.Id == id);

            if (productToDelete != null)
            {
                _context.Products.Remove(productToDelete);
                _context.SaveChanges();
            }
        }

        public Product UpdateProduct(Product product)
        {
            _context.Update(product);
            _context.SaveChanges();
            return product;
        }
    }
}
