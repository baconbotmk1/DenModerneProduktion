using System;
namespace Shared.Models
{
	public class UserRoom : BaseModel
    {
		public int UserId { get; set; }
		public User User { get; set; }
		public int RoomId { get; set; }
		public Room Room { get; set; }

        public ICollection<TimeLimit> TimeLimits { get; set; } = new List<TimeLimit>();

        public UserRoom()
		{
		}
	}
}

