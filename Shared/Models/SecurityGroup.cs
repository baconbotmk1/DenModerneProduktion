using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.Models
{
	public class SecurityGroup : TimeLimitableModel
    {
        public string Name { get; set; } = default!;
        public string? Desc { get; set; } = default;

        [InverseProperty("SecurityGroup")]
        [JsonIgnore]
        public ICollection<SecurityGroupPermission> SecurityGroupPermissions { get; } = new List<SecurityGroupPermission>();

        [InverseProperty("SecurityGroup")]
        [JsonIgnore]
        public ICollection<UserSecurityGroup> UserSecurityGroups { get; } = new List<UserSecurityGroup>();

        [NotMapped]
        public IEnumerable<Permission> Permissions => SecurityGroupPermissions.Select(e => e.Permission);
        [NotMapped]
        public IEnumerable<User> Users => UserSecurityGroups.Select(e => e.User);
	}
}

