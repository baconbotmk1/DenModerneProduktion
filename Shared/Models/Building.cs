using System;
namespace Shared.Models
{
	public class Building : BaseModel
	{
		public int CadastreId { get; set; }
		public Cadastre Cadastre { get; set; }


		public Building()
		{
		}
	}
}

