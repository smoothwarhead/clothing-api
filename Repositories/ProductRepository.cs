using KerryCoAdmin.Api.Entities.Models;
using KerryCoAdmin.Data;
using KerryCoAdmin.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace KerryCoAdmin.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }


        //check if product exists by id
        public async Task<bool> ProductExists(string productId)
        {
            return await _context.Products.AnyAsync(p => p.ProductId.ToString() == productId);
        }


        public async Task<bool> ProductExistsSlug(string slug)
        {
            return await _context.Products.AnyAsync(p => p.ProductSlug == slug);

        }

        public async Task<Product> GetProductById(string productId)
        {
            var product = await _context.Products
                            .Where(p => p.ProductId.ToString() == productId)
                            .Include(v => v.Variations)
                            .FirstOrDefaultAsync();


            return product;
        }




        public async Task<Product> GetProductBySlug(string slug)
        {
            var product = await _context.Products
                            .Where(p => p.ProductSlug == slug)
                            .Include(v => v.Variations)
                            .FirstOrDefaultAsync();


            return product;

        }



        public async Task<ICollection<Product>> GetProducts()
        {
            var products = await _context.Products                            
                            .Include(v => v.Variations)
                            .Include(d => d.Admin)
                            .ToListAsync();


            return products;

        }



       
        public async Task<string> CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            var savedProduct = product.ProductId.ToString();

            return savedProduct;            



        }



        public bool DeleteProduct(Product product)
        {

            _context.Remove(product);
            return Save();



        }


        //update product
        public async Task<bool> UpdateProduct(string id, Product product)
        {
            var savedProduct = await GetProductById(id);

            if(savedProduct != null)
            {
                savedProduct.ProductSlug = product.ProductSlug;
                savedProduct.ProductName = product.ProductName;
                savedProduct.ProductDescription = product.ProductDescription;
                savedProduct.ModifiedDate = product.ModifiedDate;
                savedProduct.Variations = product.Variations;
                

                return Save();

            }
            else
            {
                return false;
            }




        }


 



        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }



    }
}
