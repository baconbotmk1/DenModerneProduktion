using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Auth
{
    public class StartPasswordResetResult
    {
        [Required]
        public required string username { get; set; }

        [Required]
        public required string state { get; set; }
    }
}
