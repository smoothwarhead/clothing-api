namespace KerryCoAdmin.Api.Entities.Dtos.Responses
{
    public class LoginResponse
    {
        public bool Is2fa { get; set; }
        public string? Email { get; set; }
        public string? StatusType { get; set; }
        public string? Message { get; set; }
    }
}
