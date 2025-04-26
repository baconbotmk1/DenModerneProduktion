
using Microsoft.OpenApi.Models;
using Shared;
using Shared.Models;
using Shared.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text.Json.Serialization;
using SystemAPI.Services;

namespace SystemAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        MapsterConfig.RegisterMappings();

        // Add services to the container.

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            c.ExampleFilters();
        });

        builder.Services.AddDbContext<DataContext>();
        builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

        builder.Services.AddScoped<IRepository<AccessCard>,AccessCardRepository>();
        builder.Services.AddScoped<IRepository<User>, UserRepository>();
        builder.Services.AddScoped<IRepository<SecurityGroup>, SecurityGroupRepository>();
        builder.Services.AddScoped<IRepository<Building>, BuildingRepository>();
        builder.Services.AddScoped<IRepository<Room>, RoomRepository>();
        builder.Services.AddScoped<IRepository<Section>, SectionRepository>();
        builder.Services.AddScoped<IRepository<DeviceDataType>, GenericRepository<DeviceDataType>>();
        builder.Services.AddScoped<IRepository<DeviceInfoType>, GenericRepository<DeviceInfoType>>();
        builder.Services.AddScoped<IRepository<DeviceEventType>, GenericRepository<DeviceEventType>>();
        builder.Services.AddScoped<IRepository<DeviceSharedCategory>, GenericRepository<DeviceSharedCategory>>();
        builder.Services.AddScoped<IRepository<DeviceType>, GenericRepository<DeviceType>>();
        builder.Services.AddScoped<IRepository<Device>, GenericRepository<Device>>();

        builder.Services.AddScoped(sp =>
                new HttpClient
                {
                    BaseAddress = new Uri(builder.Configuration.GetSection("API").GetValue<string>("Host") ?? ""),
                });

        builder.Services.AddHttpClient("mailer", (options) =>
        {   
            options.DefaultRequestHeaders.Add("Authorization", "Bearer " + builder.Configuration.GetSection("Mailer").GetValue<string>("AuthToken"));
            options.DefaultRequestHeaders.Add("Content-Type", "application/json");
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

