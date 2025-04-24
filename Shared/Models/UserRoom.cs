using Shared.Models.Assignables.TimeLimit;
using System;
namespace Shared.Models
{
	public class UserRoom : TimeLimitableModel
    {
		public int UserId { get; set; }
		public User User { get; set; }
		public int RoomId { get; set; }
		public Room Room { get; set; }

        public ICollection<UserRoomTL> TimeLimits { get; set; } = new List<UserRoomTL>();

        public UserRoom()
		{
		}
	}
}

