using System;
namespace Shared.Models
{
	public class SecurityGroupRoom : BaseModel
    {
		public int SecurityGroupId { get; set; }
		public SecurityGroup SecurityGroup { get; set; }
		public int RoomId { get; set; }
		public Room Room { get; set; }

        public ICollection<TimeLimit> TimeLimits { get; set; } = new List<TimeLimit>();

        public SecurityGroupRoom()
		{
		}
	}
}

