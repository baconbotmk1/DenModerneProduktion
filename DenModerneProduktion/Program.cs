using DenModerneProduktion.Components;
using DenModerneProduktion.Services;
using Shared.Models;

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

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddDistributedMemoryCache((options) =>
            {

            });
            builder.Services.AddSession((options) =>
            {

            });

            builder.Services.AddBlazorBootstrap();

            builder.Services.AddScoped(sp =>
                new HttpClient
                {
                    BaseAddress = new Uri(builder.Configuration.GetSection("API").GetValue<string>("Host") ?? ""),
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

            app.UseSession();
            
            app.UseStatusCodePagesWithRedirects("/NotFound");

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
