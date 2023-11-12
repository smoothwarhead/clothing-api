namespace KerryCoAdmin.Api.Entities.Dtos.Requests
{
    public class AuthRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
