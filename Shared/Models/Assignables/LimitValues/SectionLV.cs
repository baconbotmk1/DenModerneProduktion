using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Assignables.LimitValues
{
    public class SectionLV : LimitValueAssignment
    {
        public int SectionId { get; set; }
        public Section Section { get; set; }
    }
}
