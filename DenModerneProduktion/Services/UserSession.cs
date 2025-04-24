using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Shared.DTOs.Auth;
using Shared.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace DenModerneProduktion.Services
{
    public class UserSession
    {
        private readonly ApiRequester _requester;
        private readonly HttpContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navMan;
        private readonly ViewHelper viewHelper;
        private readonly IServiceProvider _provider;

        public ClaimsPrincipal? CurrentUserClaim { get; set; }
        public User? CurrentUser { get; set; }
        public List<Permission>? Permissions { get; set; }

        public bool IsLoggedIn => CurrentUser != null && Permissions != null;

        public string Token { get; set; } = "N/A";

        public UserSession( IHttpContextAccessor contextAccessor, HttpClient httpClient, NavigationManager navMan, ApiRequester requester, IServiceProvider provider )
        {
            _provider = provider;
            _navMan = navMan;
            _httpClient = httpClient;

            Debug.WriteLine("Create UserSession: " + Token);

            _requester = requester;
            _contextAccessor = contextAccessor;
            _context = contextAccessor.HttpContext!;
        }

        async public Task Init()
        {
            var authStateProvider = _provider.GetService<AuthenticationStateProvider>();

            var authState = await authStateProvider.GetAuthenticationStateAsync();
            CurrentUserClaim = authState.User;

            if (CurrentUserClaim.Identity.IsAuthenticated)
            {
                foreach(Claim claim in CurrentUserClaim.Claims)
                {
                }

                if(CurrentUserClaim.HasClaim(c => c.Type == ClaimTypes.UserData))
                {
                    Claim userClaim = CurrentUserClaim.Claims.First(c => c.Type == ClaimTypes.UserData);
                    if(userClaim.Value != "")
                    {
                        CurrentUser = JsonSerializer.Deserialize<User>(userClaim.Value);
                    }
                }

                if (CurrentUserClaim.HasClaim(c => c.Type == "permissions"))
                {
                    Claim userClaim = CurrentUserClaim.Claims.First(c => c.Type == "permissions");
                    if (userClaim.Value != "")
                    {
                        Permissions = JsonSerializer.Deserialize<List<Permission>>(userClaim.Value);
                    }
                }

            }
            else
            {
                CurrentUser = null;
                Permissions = null;
            }

            ;
        }

        async public Task TryLoadSession()
        {
            _httpClient.BaseAddress = new Uri(_navMan.BaseUri);
            var result = await _httpClient.GetAsync("api/session/get-user");
            if(result.IsSuccessStatusCode)
            {
                var data = await result.Content.ReadFromJsonAsync<LoginResult>();
                if(data != null)
                {
                    CurrentUser = data.user;
                    Permissions = data.permissions;

                    await _provider.GetService<ViewHelper>().NavUpdate.Invoke();
                }
            }
        }

        async public Task<bool> Logout()
        {
            CurrentUser = null;
            Permissions = null;

            return true;
        }

        async public Task<bool> Login( string username, string password )
        {
            var response = await _requester.Post<LoginResult>("api/auth/login", new LoginPost()
            {
                username = username,
                password = password
            });

            if(response.IsSuccessResponse)
            {
                LoginResult result = response.TryGetData<LoginResult>()!;

                //_context.Session.SetObject("user", result.user);
                //_context.Session.SetObject("permissions", result.permissions);

                CurrentUser = result.user;
                Permissions = result.permissions;

                Token = Random.Shared.Next(1, 1000).ToString();


                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, CurrentUser.Username),
                    new Claim(ClaimTypes.Sid, CurrentUser.Id.ToString()),
                    new Claim(ClaimTypes.UserData, JsonSerializer.Serialize(CurrentUser)),
                    new Claim("permissions", JsonSerializer.Serialize(Permissions)),
                };

                foreach (var permission in Permissions)
                {
                    claims.Add(new Claim("permission", permission.Slug));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var user = new ClaimsPrincipal(identity);

                await _context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

                return true;
            }

            return false;
        }


        public bool HasPermission( string slug )
        {
            return Permissions != null && Permissions.Any(p => p.Slug.ToLower() == slug.ToLower());
        }
    }
}
