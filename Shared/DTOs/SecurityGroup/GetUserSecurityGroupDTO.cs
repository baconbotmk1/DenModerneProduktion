using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.DTOs
{
	public class GetUserSecurityGroupDTO : SecurityGroup
	{
        [JsonIgnore]
        [NotMapped]
        public override IEnumerable<User> Users => new List<User>();

        [JsonIgnore]
        [NotMapped]
        public override ICollection<UserSecurityGroup> UserSecurityGroups { get; } = new List<UserSecurityGroup>();
    }
}

