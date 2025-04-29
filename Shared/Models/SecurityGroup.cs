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
        public virtual ICollection<SecurityGroupPermission> SecurityGroupPermissions { get; } = new List<SecurityGroupPermission>();

        [InverseProperty("SecurityGroup")]
        [JsonIgnore]
        public virtual ICollection<UserSecurityGroup> UserSecurityGroups { get; } = new List<UserSecurityGroup>();

        [InverseProperty("SecurityGroup")]
        [JsonIgnore]
        public virtual ICollection<SecurityGroupRoom> SecurityGroupRooms { get; } = new List<SecurityGroupRoom>();

        [NotMapped]
        public virtual IEnumerable<Permission> Permissions => SecurityGroupPermissions.Select(e => e.Permission);
        [NotMapped]
        public virtual IEnumerable<User> Users => UserSecurityGroups.Select(e => e.User);
    }
}

