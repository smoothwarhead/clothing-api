using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace KerryCoAdmin.Api.Entities.Dtos.Responses
{
    public class VariationResponse
    {
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? NumberInPack { get; set; }
        public string? ImageUrl { get; set; }
        public string? Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }


    }
}
