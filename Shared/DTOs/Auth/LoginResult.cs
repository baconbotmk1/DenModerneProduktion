

namespace Shared.DTOs.Auth
{
    public class LoginResult
    {
        public Shared.Models.User user { get; set; }
        public List<Shared.Models.Permission> permissions { get; set; }

        public LoginResult(Models.User user, List<Models.Permission> permissions)
        {
            this.user = user;
            this.permissions = permissions;
        }
    }
}
