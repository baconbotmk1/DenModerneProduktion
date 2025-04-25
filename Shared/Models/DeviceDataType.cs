using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class DeviceDataType : BaseModel
    {
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string DataType { get; set; }

        public string? Desc { get; set; }

        [Required]
        public int CategoryId { get; set; }
		public DeviceSharedCategory Category { get; set; }

        public ICollection<DeviceDataLimitValue> LimitValues { get; set; } = new List<DeviceDataLimitValue>();


        public string GetSlug() => Category.Name + " - " + Name;

        public DeviceDataType()
		{
		}
	}
}

