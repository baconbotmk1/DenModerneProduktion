using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Auth
{
    public class StartPasswordResetPost
    {
        [Required]
        public required string username { get; set; }
    }
}
