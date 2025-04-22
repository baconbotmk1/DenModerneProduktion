using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class DeviceEventType : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public string? Desc { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public DeviceSharedCategory Category { get; set; }

        public string GetSlug() => Category.Name + " - " + Name;

        public DeviceEventType()
		{
		}
	}
}

