using System;
namespace Shared.Models.Assignables.TimeLimit
{
	public class SecurityGroupDeviceTL : TimeLimitAssignment
	{
        public int SecurityGroupDeviceId { get; set; }
        public SecurityGroupDevice SecurityGroupDevice { get; set; }
	}
}

