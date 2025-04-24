using Shared.Models.Assignables.TimeLimit;
using System;
namespace Shared.Models
{
	/// <summary>
	/// A Security Group is given access to a door through it's Device.
	/// </summary>
	public class SecurityGroupDevice : TimeLimitableModel
    {
		public int SecurityGroupId { get; set; }
		public SecurityGroup SecurityGroup { get; set; }

		public int DeviceId { get; set; }
		public Device Device { get; set; }

		public ICollection<SecurityGroupDeviceTL> TimeLimits { get; set; } = new List<SecurityGroupDeviceTL>();


        public SecurityGroupDevice()
		{
		}
	}
}

