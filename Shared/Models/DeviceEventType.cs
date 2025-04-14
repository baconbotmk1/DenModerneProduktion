using System;
namespace Shared.Models
{
	public class DeviceEventType : BaseModel
    {
		public string Name { get; set; }
		public string Desc { get; set; }

        public int CategoryId { get; set; }
        public DeviceSharedCategory Category { get; set; }

        public DeviceEventType()
		{
		}
	}
}

