using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class DeviceInfoType : BaseModel
    {
        [Required]
        public string Type { get; set; } = default!;

        [Required]
        public int CategoryId { get; set; }
        public DeviceSharedCategory Category { get; set; }

        public DeviceInfoType()
		{
		}
	}
}

