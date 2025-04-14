using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
	public class Permission : BaseModel
    {
		public string Name { get; set; }
		public string Desc { get; set; }

        [InverseProperty("Permission")]
        public ICollection<SecurityGroupPermission> SecurityGroupPermissions { get; } = new List<SecurityGroupPermission>();


		public Permission()
		{
		}
	}
}

