using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.DTOs.SecurityGroup
{
	public class GetUserSecurityGroupDTO : Models.SecurityGroup
	{
        [JsonIgnore]
        [NotMapped]
        public override IEnumerable<Models.User> Users => new List<Models.User>();

        [JsonIgnore]
        [NotMapped]
        public override ICollection<UserSecurityGroup> UserSecurityGroups { get; set; } = new List<UserSecurityGroup>();
    }
}

