using KerryCoAdmin.Entities.Dtos.Response;

namespace KerryCoAdmin.Api.Entities.Dtos.Responses
{
    public class FinalAuthResponse
    {
        public bool IsAuth { get; set; }
        public bool Is2fa { get; set; }
        public string? Token { get; set; }
        public ICollection<string>? Role { get; set; }
        public string? StatusType { get; set; }
        public string? Message { get; set; }
        public AdminResponse? User { get; set; }
    }
}
