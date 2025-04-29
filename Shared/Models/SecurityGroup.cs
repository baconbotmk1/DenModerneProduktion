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

        //[JsonIgnore]
        [InverseProperty("SecurityGroup")]
        public virtual ICollection<SecurityGroupPermission> SecurityGroupPermissions { get; set; } = new List<SecurityGroupPermission>();

        //[JsonIgnore]
        [InverseProperty("SecurityGroup")]
        public virtual ICollection<UserSecurityGroup> UserSecurityGroups { get; set; } = new List<UserSecurityGroup>();

        //[JsonIgnore]
        [InverseProperty("SecurityGroup")]
        public virtual ICollection<SecurityGroupRoom> SecurityGroupRooms { get; set; } = new List<SecurityGroupRoom>();
    }
}

