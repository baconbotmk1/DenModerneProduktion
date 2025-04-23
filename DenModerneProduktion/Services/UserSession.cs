using Microsoft.AspNetCore.Http;

namespace DenModerneProduktion.Services
{
    public class UserSession
    {
        public string Token { get; set; } = string.Empty;

        public UserSession( IHttpContextAccessor contextAccessor)
        {
            HttpContext context = contextAccessor.HttpContext;

            if (!context.Session.Keys.Contains("token"))
            {
                context.Session.SetString("token", string.Join("", Random.Shared.GetItems<char>("abcdefghijklmnopqrstuvwxyz".ToCharArray(), 5)));
            }
            Token = context.Session.GetString("token") ?? "N/A";
        }
    }
}
