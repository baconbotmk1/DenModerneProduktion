using System;
namespace Shared.Models
{
	public class SecurityGroupPermission : BaseModel
    {
		public int SecurityGroupId { get; set; }
		public SecurityGroup SecurityGroup { get; set; }
		public int PermissionId { get; set; }
		public Permission Permission { get; set; }


		public SecurityGroupPermission()
		{
		}
	}
}

