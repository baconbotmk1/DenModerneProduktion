using System;
namespace Shared.Models
{
	public class Cadastre : BaseModel
	{
		public ICollection<Building> Buildings { get; set; } = new List<Building>();


		public Cadastre()
		{
		}
	}
}

