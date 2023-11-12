using KerryCoAdmin.Api.Entities.Models;
using KerryCoAdmin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace KerryCoAdmin.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        public DbSet<Admin> Admins { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariation> ProductVariations { get; set; }







        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           

            SeedRoles(builder);
        }


        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
             (
                new IdentityRole() { Name = "SuperAdmin", ConcurrencyStamp = "1", NormalizedName = "SuperAdmin" },
                new IdentityRole() { Name = "Staff", ConcurrencyStamp = "2", NormalizedName = "Staff" }
           
             );

        }



    }
}
