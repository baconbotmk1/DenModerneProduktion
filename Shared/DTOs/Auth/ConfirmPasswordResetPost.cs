using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Auth
{
    public class ConfirmPasswordResetPost
    {
        [Required]
        public required string token { get; set; }

        [Required]
        public required string password { get; set; }
    }
}
