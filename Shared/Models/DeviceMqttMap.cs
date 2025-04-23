using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class DeviceMqttMap : BaseModel
    {
        [Required]
        public int DeviceTypeId { get; set; }
        public DeviceType DeviceType { get; set; }

        [Required]
        public string FieldName { get; set; }

        public int? DataTypeId { get; set; }
		public DeviceDataType? DataType { get; set; }

        public int? InfoTypeId { get; set; }
        public DeviceInfoType? InfoType { get; set; }

        public DeviceMqttMap()
		{
		}
	}
}

