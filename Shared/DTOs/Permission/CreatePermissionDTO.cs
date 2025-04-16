using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.DTOs
{
	public class CreatePermissionDTO
	{
		[Required]
		public required string Name { get; set; }
        public string? Desc { get; set; }
    }
}

