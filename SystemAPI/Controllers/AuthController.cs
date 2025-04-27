using Microsoft.AspNetCore.Identity.Data;
using Shared.DTOs.AccessCard;
using Shared.DTOs.Auth;
using Shared.Migrations;
using System.Security.Cryptography;
using System.Text;
using SystemAPI.Helpers;

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
        public ActionResult<LoginResult> TryLogin([FromBody] LoginPost data)
        {
            User? user = context.Users
                .AsQueryable()
                .Include(e => e.UserSecurityGroups)
                    .ThenInclude(e2 => e2.SecurityGroup)
                        .ThenInclude(e3 => e3.SecurityGroupPermissions)
                            .ThenInclude(e4 => e4.Permission)
                .FirstOrDefault(e => e.Username.ToLower() == data.username.ToLower());
            if (user == null)
            {
                return NotFound();
            }

            if(user.HashedPassword == null || user.Salt == null)
            {
                return NotFound();
            }

            byte[] hashedPasswordBytes = Convert.FromBase64String(user.HashedPassword);
            byte[] saltBytes = Convert.FromBase64String(user.Salt);

            if (!CryptoHelper.ComparePasswordWithHash(data.password, hashedPasswordBytes, saltBytes))
            {
                return NotFound();
            }

            List<Permission> permissions = user.SecurityGroups.SelectMany(e => e.Permissions)
                    .Distinct()
                    .ToList();

            return Ok(new LoginResult(user, permissions));
        }


        [HttpPost("reset_password")]
        public ActionResult<StartPasswordResetResult> Post([FromBody] StartPasswordResetPost data)
        {
            User? user = context.Users.FirstOrDefault(e => e.Username.ToLower() == data.username.ToLower());
            if (user == null)
            {
                return NotFound();
            }

            string token = CryptoHelper.GenerateSaltString();
            string state = CryptoHelper.GenerateSaltString(); //Delete?

            user.ResetToken = token;

            //Send reset token as email.

            context.Attach(user);
            context.SaveChanges();

            return Ok(new ConfirmPasswordResetResult() {
                state = state,
                username = data.username,
            });
        }


        [HttpPost("reset_password/{token}")]
        public ActionResult Post(string token, [FromBody] ConfirmPasswordResetPost data)
        {
            User? user = context.Users.FirstOrDefault(e => e.ResetToken == token);
            if (user == null)
            {
                return Ok();
            }

            byte[] salt = user.Salt != null ? Convert.FromBase64String(user.Salt) : CryptoHelper.GenerateSalt();

            if (user.Salt == null)
            {
                user.Salt = Convert.ToBase64String(salt);
            }
            user.ResetToken = null;
            user.ResetState = null; //delete?
            user.HashedPassword = Convert.ToBase64String(CryptoHelper.HashPassword(data.password, salt));

            context.Attach(user);
            context.SaveChanges();

            return Ok();
        }
    }
}

