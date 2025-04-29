using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.Models
{
	public class Permission : BaseModel
    {
        public string Slug { get; set; }
        public string Name { get; set; }
		public string Desc { get; set; }

        //[JsonIgnore]
        [InverseProperty("Permission")]
        public ICollection<SecurityGroupPermission> SecurityGroupPermissions { get; set; } = new List<SecurityGroupPermission>();

        public Permission()
		{
		}
	}
}

