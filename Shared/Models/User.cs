using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Shared.Models
{
	public class User : TimeLimitableModel
    {
		public string Name { get; set; } = default!;
        public bool IsActive { get; set; } = false;

		public string Username { get; set; }
		public string? HashedPassword { get; set; }
		public string? Salt { get; set; }
		public string? ResetToken { get; set; }

        public string? ReferenceId { get; set; }
		public string? ReferenceType { get; set; }

        [InverseProperty("User")]
		[JsonIgnore]
		public virtual ICollection<UserSecurityGroup> UserSecurityGroups { get; set; } = new List<UserSecurityGroup>();

		public virtual ICollection<AccessCard> AccessCards { get; set; } = new List<AccessCard>();

        [NotMapped]
        public virtual IEnumerable<SecurityGroup> SecurityGroups => UserSecurityGroups.Select(e => e.SecurityGroup);

        [InverseProperty("User")]
        [JsonIgnore]
        public virtual ICollection<UserRoom> UserRooms { get; set; } = new List<UserRoom>();


        public User()
		{
		}
	}
}

