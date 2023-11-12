namespace KerryCoAdmin.Api.Entities.Dtos.Requests
{
    public class ForgotPasswordRequest
    {
        public string? Email { get; set; }
        public string? ClientURI { get; set; }
    }
}
