using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.DTOs.Permission
{
	public class CreatePermissionDTO
	{
		[Required]
		public required string Name { get; set; }
        public string? Desc { get; set; }
    }
}

