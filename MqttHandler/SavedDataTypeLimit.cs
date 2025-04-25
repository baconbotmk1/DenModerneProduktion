using System;
namespace MqttHandler
{
	public class SavedDataTypeLimit
	{
        public required int Id { get; set; }

        public required int RefId { get; set; }
		public required string RefType { get; set; }

		public double? Minimum { get; set; }
        public double? Maximum { get; set; }
    }
}

