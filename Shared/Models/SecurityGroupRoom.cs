using System;
namespace Shared.Models
{
	public class SecurityGroupRoom : TimeLimitableModel
    {
		public int SecurityGroupId { get; set; }
		public SecurityGroup SecurityGroup { get; set; }
		public int RoomId { get; set; }
		public Room Room { get; set; }

		public SecurityGroupRoom()
		{
		}
	}
}

