using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Auth
{
    public class LoginPost
    {
        [Required]
        public required string username { get; set; }

        [Required]
        public required string password { get; set; }
    }
}
