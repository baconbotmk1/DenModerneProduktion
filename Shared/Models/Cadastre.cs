using Shared.Models.Assignables.LimitValues;
using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class Cadastre : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public ICollection<Building> Buildings { get; set; } = new List<Building>(); 
        public ICollection<DeviceDataLimitValue> LimitValues { get; set; } = new List<DeviceDataLimitValue>();

        public Cadastre()
		{
		}
	}
}

