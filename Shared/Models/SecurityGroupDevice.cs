using System;
namespace Shared.Models
{
	/// <summary>
	/// A Security Group is given access to a door through it's Device.
	/// </summary>
	public class SecurityGroupDevice : BaseModel
    {
		public int SecurityGroupId { get; set; }
		public SecurityGroup SecurityGroup { get; set; }

		public int DeviceId { get; set; }
		public Device Device { get; set; }

		public ICollection<TimeLimit> TimeLimits { get; set; } = new List<TimeLimit>();


        public SecurityGroupDevice()
		{
		}
	}
}

