using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class DeviceDataType : BaseModel
    {
        [Required]
        public string Type { get; set; } = default!;
        [Required]
        public string DataType { get; set; }

        [Required]
        public int CategoryId { get; set; }
		public DeviceSharedCategory Category { get; set; }

		public DeviceDataType()
		{
		}
	}
}

