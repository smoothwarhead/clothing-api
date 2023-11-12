using KerryCoAdmin.Api.Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace KerryCoAdmin.Entities.Dtos.Request
{
    public class ProductRequest
    {

        public string? Id { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }   
        public string? ProductSlug { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? NumberInPack { get; set; }
        public string? ImageUrl { get; set; }
        public string? Url { get; set; }
        public string? SecureUrl { get; set; }
        public string? Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }

    }
}
