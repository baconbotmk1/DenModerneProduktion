using System;
namespace Shared.Models
{
	public class UnknownMqttDevices : BaseModel
	{
        public required string Identifier { get; set; }

        public required string LastPayload { get; set; }

		public DateTime FirstFoundAt { get; set; }

        public required DateTime LastFoundAt { get; set; }
    }
}

