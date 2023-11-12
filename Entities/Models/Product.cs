using KerryCoAdmin.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace KerryCoAdmin.Api.Entities.Models
{
    public class Product
    {
        public Guid ProductId { get; set; } = new Guid();
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public string? ProductSlug { get; set; }
       

        // Navigation Properties
        public Admin? Admin { get; set; }
        public ICollection<ProductVariation>? Variations  { get; set; }


    }
}
