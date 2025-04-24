using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Assignables.LimitValues
{
    public class RoomLV : LimitValueAssignment
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
