using System;
namespace Shared.Models
{
	public class DeviceData : BaseModel
    {
		public string Value { get; set; }

		public int DeviceId { get; set; }
        public Device Device { get; set; }
        public int TypeId { get; set; }
		public DeviceDataType Type { get; set; }

		public DeviceData()
		{
		}
	}
}

