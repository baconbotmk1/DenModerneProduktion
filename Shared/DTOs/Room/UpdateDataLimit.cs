using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Room
{
    public class UpdateDataLimit
    {
        public double? Min { get; set; }
        public double? Max { get; set; }
        [Required]
        public int DataTypeId { get; set; }
    }
}
