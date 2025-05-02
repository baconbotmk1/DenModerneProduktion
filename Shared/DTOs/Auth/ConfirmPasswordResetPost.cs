using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Auth
{
    public class ConfirmPasswordResetPost
    {
        /// <summary>
        /// The token sent in the email
        /// </summary>
        [Required]
        public required string token { get; set; }

        /// <summary>
        /// The new password
        /// </summary>
        [Required]
        public required string password { get; set; }
    }
}
