using System;
namespace Shared.Models
{
	public class DeviceRecording : BaseModel
    {
		public DateOnly Date { get; set; }
		public DateTime Started { get; set; }
		public float? Duration { get; set; }
		public string Filepath { get; set; }
		public bool Finished { get; set; } = false;
        public bool Converted { get; set; } = false;

		public int DeviceId { get; set; }
		public Device Device { get; set; }

		public DeviceRecording()
		{
		}
	}
}

