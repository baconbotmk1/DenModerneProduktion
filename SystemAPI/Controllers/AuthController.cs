using Microsoft.AspNetCore.Identity.Data;
using Shared.DTOs.AccessCard;
using Shared.DTOs.Auth;
using Shared.Migrations;
using System.Security.Cryptography;
using System.Text;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        public AuthController(DataContext Context) : base(Context)
        {
        }


        [HttpPost("login")]
        public ActionResult<LoginResult> Post([FromBody] LoginPost data)
        {
            //byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            if (!context.Users.Any(e => e.Username.ToLower() == data.username.ToLower()))
            {
                return NotFound();
            }

            return Ok(new LoginResult());
        }
    }
}

