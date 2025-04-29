using System;
namespace Shared.DTOs.Auth
{
	public class StartFingerprint
	{
		public required int UserId { get; set; }
		public required string Identifier { get; set; }
	}
}

