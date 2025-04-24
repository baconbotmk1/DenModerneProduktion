using System;
namespace Shared.Models.Assignables.TimeLimit
{
	public class SecurityGroupRoomTL : TimeLimitAssignment
	{
        public int SecurityGroupRoomId { get; set; }
        public SecurityGroupRoom SecurityGroupRoom { get; set; }
	}
}

