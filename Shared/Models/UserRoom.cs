using System;
namespace Shared.Models
{
	public class UserRoom : TimeLimitableModel
    {
		public int UserId { get; set; }
		public User User { get; set; }
		public int RoomId { get; set; }
		public Room Room { get; set; }

		public UserRoom()
		{
		}
	}
}

