using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Assignables.LimitValues
{
    public class BuildingLV : LimitValueAssignment
    {
        public int BuildingId { get; set; }
        public Building Building { get; set; }
    }
}
