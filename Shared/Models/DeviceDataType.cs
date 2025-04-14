using System;
namespace Shared.Models
{
	public class DeviceDataType : BaseModel
    {
        public string Type { get; set; } = default!;
		public string DataType { get; set; }

		public int CategoryId { get; set; }
		public DeviceSharedCategory Category { get; set; }

		public DeviceDataType()
		{
		}
	}
}

