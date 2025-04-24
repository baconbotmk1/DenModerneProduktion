using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Assignables.LimitValues
{
    public class CadastreLV : LimitValueAssignment
    {
        public int CadastreId { get; set; }
        public Cadastre Cadastre { get; set; }
    }
}
