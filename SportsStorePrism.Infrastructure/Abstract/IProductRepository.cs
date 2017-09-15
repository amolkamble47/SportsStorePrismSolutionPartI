using System.Threading.Tasks;
using System.Collections.Generic;

using SportsStorePrism.Infrastructure.Entities;

namespace SportsStorePrism.Infrastructure.Abstract
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();
        Task<List<Product>> GetProductsByCategoryAsync(string categoryName);
        Task<Product> GetProductAsync(int productId);
        Task<Product> AddProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productId);
    }
}
