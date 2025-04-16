using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Room
{
    public class CreateRoom
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int SectionId { get; set; }
    }
}
