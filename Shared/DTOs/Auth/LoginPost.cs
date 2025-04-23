using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Auth
{
    public class LoginPost
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}
