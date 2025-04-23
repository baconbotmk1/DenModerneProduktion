using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Auth
{
    public class LoginResult
    {
        public Shared.Models.User user { get; set; }

        //public Shared.Models.UserSession userSession { get; set; }
    }
}
