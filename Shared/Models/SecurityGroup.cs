using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
	public class SecurityGroup : TimeLimitableModel
    {
		public string Name { get; set; }
		public string Desc { get; set; }

		[InverseProperty("SecurityGroup")]
        public ICollection<SecurityGroupPermission> SecurityGroupPermissions { get; } = new List<SecurityGroupPermission>();

		public IEnumerable<Permission> Permissions => SecurityGroupPermissions.Select(e => e.Permission);


        public SecurityGroup()
		{
		}
	}
}

