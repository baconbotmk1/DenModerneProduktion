using System;
namespace Shared.Models
{
	public class DeviceInfoType : BaseModel
    {
        public string Type { get; set; } = default!;

        public int CategoryId { get; set; }
        public DeviceSharedCategory Category { get; set; }

        public DeviceInfoType()
		{
		}
	}
}

