using System;
namespace Shared.Models.Assignables
{
	public class UserRoomTL : TimeLimitAssignment
	{
        public int UserRoomId { get; set; }
        public UserRoom UserRoom { get; set; }
	}
}

