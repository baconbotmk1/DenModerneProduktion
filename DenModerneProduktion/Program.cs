using DenModerneProduktion.Components;
using DenModerneProduktion.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Shared.Models;
using System.Net.Http;

namespace DenModerneProduktion
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                });
            builder.Services.AddAuthorization();
            builder.Services.AddCascadingAuthenticationState();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddBlazorBootstrap();

            builder.Services.AddHttpClient("api", sp =>
            {
                sp.BaseAddress = new Uri(builder.Configuration.GetSection("API").GetValue<string>("Host") ?? "");
            });

            builder.Services.AddScoped<ViewHelper>();

            builder.Services.AddScoped<ApiRequester>();
            builder.Services.AddScoped<UserSession>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseStatusCodePagesWithRedirects("/NotFound");

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
