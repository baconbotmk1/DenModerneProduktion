using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.DTOs
{
	public class GetUserDTO : User
	{
        [JsonIgnore]
        public override ICollection<UserSecurityGroup> UserSecurityGroups { get; set; } = new List<UserSecurityGroup>();

        //[JsonIgnore]
        public new IEnumerable<GetUserSecurityGroupDTO> SecurityGroups { get; set; } = new List<GetUserSecurityGroupDTO>();
    }
}

