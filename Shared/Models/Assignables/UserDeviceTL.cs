using System;
namespace Shared.Models.Assignables
{
	public class UserDeviceTL : TimeLimitAssignment
	{
        public int UserDeviceId { get; set; }
        public UserDevice UserDevice { get; set; }
	}
}

