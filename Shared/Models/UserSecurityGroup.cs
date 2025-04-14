using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace Shared.Models
{
	public class UserSecurityGroup : BaseModel
    {
		public int UserId { get; set; }
        [InverseProperty("UserSecurityGroups")]
        public User User { get; set; }

		public int SecurityGroupId { get; set; }
		[InverseProperty("UserSecurityGroups")]
		public SecurityGroup SecurityGroup { get; set; }

		public UserSecurityGroup()
		{
		}
	}
}

