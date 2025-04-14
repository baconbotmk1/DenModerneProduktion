using System;
namespace Shared.Models
{
	public class UserRoom : TimeLimitableModel
    {
		public int UserId { get; set; }
		public User User { get; set; }
		public int RoomId { get; set; }
		public Room Room { get; set; }

        public ICollection<Assignables.UserRoomTL> TimeLimits { get; set; } = new List<Assignables.UserRoomTL>();

        public UserRoom()
		{
		}
	}
}

