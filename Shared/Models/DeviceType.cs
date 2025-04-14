using System;
namespace Shared.Models
{
	public class DeviceType : BaseModel
    {
		public string Name { get; set; }
        public string Desc { get; set; }

        public DeviceType()
		{
		}
	}
}

