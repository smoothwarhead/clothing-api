namespace KerryCoAdmin.Api.Entities.Dtos.Responses
{
    public class InitialAuthResponse
    {
        public bool IsAuth { get; set; }
        public string? Token { get; set; }
        public string? StatusType { get; set; }
        public string? Message { get; set; }
        
    }
}
