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
        public double? MinimumLimit { get; set; }
        public double? MaximumLimit { get; set; }
        [Required]
        public int DataTypeId { get; set; }
    }
}
