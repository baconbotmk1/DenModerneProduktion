using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class DeviceInfo : BaseModel
    {
        [Required]
        public string Value { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required]
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        [Required]
        public int TypeId { get; set; }
		public DeviceInfoType Type { get; set; }

		public DeviceInfo()
		{
		}
	}
}

