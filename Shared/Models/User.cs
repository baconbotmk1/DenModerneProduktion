using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.Models
{
	public class User : TimeLimitableModel
    {
		public string Name { get; set; } = default!;
        public bool IsActive { get; set; } = false;

		public string? ReferenceId { get; set; }
		public string? ReferenceType { get; set; }

        [InverseProperty("User")]
		[JsonIgnore]
		public ICollection<UserSecurityGroup> UserSecurityGroups { get; set; } = new List<UserSecurityGroup>();

		public ICollection<AccessCard> AccessCards { get; set; } = new List<AccessCard>();

        [NotMapped]
        public IEnumerable<SecurityGroup> SecurityGroups => UserSecurityGroups.Select(e => e.SecurityGroup);


        public User()
		{
		}
	}
}

