using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
	public class User : TimeLimitableModel
    {
		public string Name { get; set; }

		public string? ReferenceId { get; set; }
		public string? ReferenceType { get; set; }

		[InverseProperty("User")]
		public ICollection<UserSecurityGroup> UserSecurityGroups { get; set; } = new List<UserSecurityGroup>();

		public User()
		{
		}
	}
}

