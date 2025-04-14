using System;
namespace Shared.Models
{
	public class DeviceInfo : BaseModel
    {
		public string Value { get; set; }

		public int DeviceId { get; set; }
        public Device Device { get; set; }
        public int TypeId { get; set; }
		public DeviceInfoType Type { get; set; }

		public DeviceInfo()
		{
		}
	}
}

