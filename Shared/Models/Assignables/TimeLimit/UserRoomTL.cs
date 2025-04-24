using System;
namespace Shared.Models.Assignables.TimeLimit
{
	public class UserRoomTL : TimeLimitAssignment
	{
        public int UserRoomId { get; set; }
        public UserRoom UserRoom { get; set; }
	}
}

