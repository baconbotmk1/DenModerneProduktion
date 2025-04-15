
using Microsoft.OpenApi.Models;
using Shared;
using Shared.Models;
using Shared.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text.Json.Serialization;

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

