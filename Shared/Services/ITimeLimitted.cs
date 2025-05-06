using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface ITimeLimitted
    {
        public abstract ICollection<TimeLimit> TimeLimits { get; set; }

        public bool IsActive( DateTime time )
        {
            return TimeLimits.Any(e => e.IsActive(time));
        }
    }
}
