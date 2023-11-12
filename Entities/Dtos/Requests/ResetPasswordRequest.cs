using System.ComponentModel.DataAnnotations;

namespace KerryCoAdmin.Api.Entities.Dtos.Requests
{
    public class ResetPasswordRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? NewPassword { get; set; }




        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmNewPassword { get; set; }

        [Required]
        public string? Token { get; set; }
    }
}
