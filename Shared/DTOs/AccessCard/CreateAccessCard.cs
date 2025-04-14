using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.AccessCard
{
	public class CreateAccessCard
	{
        [Required]
        public string Name { get; set; }

        [Required]
        public string UniqueCode { get; set; }

        public int? UserId { get; set; }
    }
}

