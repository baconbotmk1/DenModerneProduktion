using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.Models
{
	public class Permission : BaseModel
    {
        public string Name { get; set; }
		public string Desc { get; set; }

        [InverseProperty("Permission")]
        [JsonIgnore]
        public ICollection<SecurityGroupPermission> SecurityGroupPermissions { get; } = new List<SecurityGroupPermission>();

        [NotMapped]
        public IEnumerable<SecurityGroup> SecurityGroups => SecurityGroupPermissions.Select(e => e.SecurityGroup);


        public Permission()
		{
		}
	}
}

