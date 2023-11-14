
using KerryCoAdmin.Api.Entities.Models;

namespace KerryCoAdmin.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> ProductExists(string productId);
        Task<bool> ProductExistsSlug(string slug);

        Task<Product> GetProductById(string productId);
        Task<Product> GetProductBySlug(string slug);

        Task<ICollection<Product>> GetProducts();

      

        Task<string> CreateProduct(Product product);
    
        
        bool DeleteProduct(Product product);
        Task<bool> UpdateProduct(string id, Product product);

        bool Save();


    }
}
