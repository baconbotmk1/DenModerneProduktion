using System;
namespace Shared.Models
{
	public class UserSecurityGroup : BaseModel
    {
		public int UserId { get; set; }
		public User User { get; set; }
		public int SecurityGroupId { get; set; }
		public SecurityGroup SecurityGroup { get; set; }

		public UserSecurityGroup()
		{
		}
	}
}

