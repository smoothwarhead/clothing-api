

using KerryCoAdmin.Api.Entities.Dtos.Responses;
using KerryCoAdmin.Api.Entities.Models;

namespace KerryCoAdmin.Entities.Dtos.Response
{
    public class ProductResponse
    {
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ProductSlug { get; set; }
        public ICollection<VariationResponse>? Variations { get; set; }
        public AdminProfileResponse? Admin { get; set; }


    }


}
