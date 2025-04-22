using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class DeviceSharedCategory : BaseModel
    {
        [Required]
        public string Name { get; set; }
		public string Desc { get; set; }

        public ICollection<DeviceInfoType> InfoTypes { get; set; } = new List<DeviceInfoType>();
        public ICollection<DeviceDataType> DataTypes { get; set; } = new List<DeviceDataType>();
        public ICollection<DeviceEventType> EventTypes { get; set; } = new List<DeviceEventType>();

        public DeviceSharedCategory()
		{
		}
	}
}

