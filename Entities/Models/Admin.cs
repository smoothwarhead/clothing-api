using KerryCoAdmin.Api.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace KerryCoAdmin.Models
{
    public class Admin
    {
        public Guid AdminId { get; set; } = new Guid();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
       
        public IdentityUser? User { get; set; }
        public ICollection<Product>? Products { get; set; }



    }
}
