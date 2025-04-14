using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
{
	public class DeviceRecording : BaseModel
    {
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public DateTime Started { get; set; }
		public float? Duration { get; set; }
        [Required]
        public string Filepath { get; set; }
		public bool Finished { get; set; } = false;
        public bool Converted { get; set; } = false;

        [Required]
        public int DeviceId { get; set; }
		public Device Device { get; set; }

		public DeviceRecording()
		{
		}
	}
}

