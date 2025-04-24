using System;
namespace Shared.Models.Assignables.TimeLimit
{
	public class UserDeviceTL : TimeLimitAssignment
	{
        public int UserDeviceId { get; set; }
        public UserDevice UserDevice { get; set; }
	}
}

