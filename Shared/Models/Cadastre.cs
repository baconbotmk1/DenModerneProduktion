using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class Cadastre : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Building> Buildings { get; set; } = new List<Building>(); 
        public virtual ICollection<DeviceDataLimitValue> LimitValues { get; set; } = new List<DeviceDataLimitValue>();

        public Cadastre()
		{
		}
	}
}

