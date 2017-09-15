using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;

using SportsStorePrism.Infrastructure.Abstract;
using SportsStorePrism.Infrastructure.Entities;

namespace SportsStorePrism.Module.Services
{
    public class EfProductRepository : IProductRepository
    {
        private SportsStoreDbContext _context;
        public EfProductRepository()
        {
            _context = new SportsStoreDbContext();
        }
        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProductAsync(int productId)
        {
            var prod = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (prod != null)
            {
                _context.Products.Remove(prod);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            var prod = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            return prod;
        }

        public Task<List<Product>> GetProductsAsync()
        {
            return _context.Products.ToListAsync();
        }

        public Task<List<Product>> GetProductsByCategoryAsync(string categoryName)
        {
            return _context.Products.Where(p => p.Category == categoryName).ToListAsync();
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            if (!_context.Products.Local.Any(p => p.ProductId == product.ProductId))
            {
                _context.Products.Attach(product);
            }
            _context.Entry<Product>(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
