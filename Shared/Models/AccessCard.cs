using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Shared.Models
{
	public class AccessCard : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        /// <summary>
        /// This code is the unique identifier, that tells us who they are. This could be a RFID code.
        /// </summary>
        public string UniqueCode { get; set; }


		public int? UserId { get; set; }
        public User? User { get; set; }
	}
}

