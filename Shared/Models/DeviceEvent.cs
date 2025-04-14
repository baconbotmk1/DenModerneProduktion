using System;
namespace Shared.Models
{
	public class DeviceEvent : BaseModel
    {
		public DateTime Timestamp { get; set; }
		public string? Desc { get; set; }

        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public int TypeId { get; set; }
		public DeviceEventType Type { get; set; }

		public DeviceEvent()
		{
		}
	}
}

