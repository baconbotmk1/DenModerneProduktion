using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class Building : BaseModel
	{
        [Required]
        public int CadastreId { get; set; }
		public Cadastre Cadastre { get; set; }


		public Building()
		{
		}
	}
}

