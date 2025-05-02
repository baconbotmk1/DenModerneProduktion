using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.User
{
    public class ImportedUser
    {
        public string Uuid { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public int? Age { get; set; } = null;
    }
}
