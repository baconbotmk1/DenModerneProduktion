using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.DTOs
{
	public class CreateSecurityGroupDTO
	{
		[Required]
		public required string Name { get; set; }
		public string? Desc { get; set; }
	}
}

