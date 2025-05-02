using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
	public class TimeLimit : BaseModel
    {
		public string Name { get; set; } = "";

        public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }

		public ICollection<TimeLimitWeek> Weeks { get; set; } = new List<TimeLimitWeek>();

        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public int? SecurityGroupId { get; set; }
        public SecurityGroup? SecurityGroup { get; set; }

        public int? SecurityGroupDeviceId { get; set; }
        public SecurityGroupDevice? SecurityGroupDevice { get; set; }

        public int? SecurityGroupRoomId { get; set; }
        public SecurityGroupRoom? SecurityGroupRoom { get; set; }

        public int? SecurityGroupSectionId { get; set; }
        public SecurityGroupSection? SecurityGroupSection { get; set; }

        public int? UserDeviceId { get; set; }
        public UserDevice? UserDevice { get; set; }

        public int? UserRoomId { get; set; }
        public UserRoom? UserRoom { get; set; }

        public int? UserSectionId { get; set; }
        public UserSection? UserSection { get; set; }


        public TimeLimit()
		{
		}


        public bool IsActive( DateTime time )
        {
            if (FromDate != null && FromDate > time)
                return false;
            if (ToDate != null && ToDate < time)
                return false;

            if(Weeks.Count > 0)
            {
                foreach (TimeLimitWeek week in Weeks)
                {
                    if (week.IsActive(time))
                        return true;
                }

                return false;
            }


            return true;
        }
	}
}

