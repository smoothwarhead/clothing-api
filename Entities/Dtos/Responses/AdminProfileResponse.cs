

namespace KerryCoAdmin.Entities.Dtos.Response
{
    public class AdminProfileResponse
    {
        public string? AdminId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }       
        public UserResponse? User { get; set; }
    }
}
