using Shared;

namespace MqttHandler;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddDbContext<DataContext>();

        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();
        host.Run();
    }
}
