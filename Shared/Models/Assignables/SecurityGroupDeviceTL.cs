using System;
namespace Shared.Models.Assignables
{
	public class SecurityGroupDeviceTL : TimeLimitAssignment
	{
        public int SecurityGroupDeviceId { get; set; }
        public SecurityGroupDevice SecurityGroupDevice { get; set; }
	}
}

