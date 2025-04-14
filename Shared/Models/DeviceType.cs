using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class DeviceType : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string Desc { get; set; }

        public DeviceType()
		{
		}
	}
}

