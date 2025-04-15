using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Shared.DTOs
{
	public class CreateUser
	{
		[Required]
		public required string Name { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool IsActive { get; set; } = false;
        public string? ReferenceId { get; set; }
        public string? ReferenceType { get; set; }
    }
}

