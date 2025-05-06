using System.Globalization;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MQTTnet;
using Shared;
using Shared.Models;

namespace SystemAPI.Services;

/// <summary>
/// Background service that listens to MQTT messages and handles them.
/// </summary>
public class MqttReacter : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private IMqttClient _mqttClient;

    private DateTime? lastDataUpdate = null;

    private ICollection<Device> AuthDevices = new List<Device>();


    public MqttReacter(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("[MQTT-BGS] Starting background service");

        var factory = new MqttClientFactory();
        _mqttClient = factory.CreateMqttClient();

        IConfigurationSection mqttSection = _configuration.GetSection("Mqtt");

        Console.WriteLine("[MQTT-BGS] Building settings...");

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(mqttSection.GetValue<string>("Host"), mqttSection.GetValue<int>("Port"))
            .WithTlsOptions(new MqttClientTlsOptions()
            {
                UseTls = true
            })
            .WithCredentials(mqttSection.GetValue<string>("Username"), mqttSection.GetValue<string>("Password"))
            .Build();

        _mqttClient.ConnectedAsync += async (MqttClientConnectedEventArgs e) =>
        {
            Console.WriteLine("[MQTT-BGS] Connected to MQTT Broker.");
            //await _mqttClient.SubscribeAsync("#");
            await _mqttClient.SubscribeAsync("zigbee2mqtt/+");
        };

        _mqttClient.ApplicationMessageReceivedAsync += async (MqttApplicationMessageReceivedEventArgs e) =>
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

            Console.WriteLine($"\n[MQTT-BGS] Got message");

            if(lastDataUpdate == null || DateTime.UtcNow > lastDataUpdate + TimeSpan.FromSeconds(30))
            {
                await UpdateData();
            }

            string identifier = topic;

            Console.WriteLine($"[MQTT-BGS] Received: {topic} - {payload}");

            if (identifier.Contains("zigbee2mqtt"))
            {
                string[] splitted = identifier.Split('/');
                identifier = splitted[1];

                if (!identifier.StartsWith("0x") || splitted.Count() > 2 || splitted.Count() < 2)
                {
                    return;
                }

                await HandleZigbee2Mqtt(identifier, topic, payload);
            }
        };

        Console.WriteLine("[MQTT-BGS] Connecting to broker...");

        await _mqttClient.ConnectAsync(options, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_mqttClient?.IsConnected == true)
        {
            await _mqttClient.DisconnectAsync();
        }

        await base.StopAsync(cancellationToken);
    }

    public async Task UpdateData()
    {
        lastDataUpdate = DateTime.UtcNow;

        Console.WriteLine("\n[MQTT-BGS] Updating data...");

        using var scope = _scopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        AuthDevices = context.Devices.AsQueryable()
            .Where(e => e.TypeId == 7)
            .ToList();

        Console.WriteLine("[MQTT-BGS] Data updated\n");
    }

    public bool CheckCanSaveUnknown()
    {
        using var scope = _scopeFactory.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<DataContext>();

        try
        {
            db.UnknownMqttDevices.AsQueryable().Count();
        }
        catch(MySqlConnector.MySqlException ex)
        {
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }


    public async Task HandleZigbee2Mqtt( string identifier, string topic, string payload )
    {
        using var scope = _scopeFactory.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<DataContext>();

        DateTime currentTime = DateTime.UtcNow;

        Console.WriteLine($"[MQTT-BGS] Known Zigbee2MQTT - {identifier} - {payload}");

        {
            Device? authDevice = AuthDevices.FirstOrDefault(e => e.Identifier == identifier);
            if(authDevice != null)
            {
                RFID rfid = JsonSerializer.Deserialize<RFID>(payload);
                rfid.rfid = rfid.rfid.ToUpper(CultureInfo.InvariantCulture);

                var requester = scope.ServiceProvider.GetRequiredService<MqttRequester>();

                db.Devices.AsQueryable()
                    .Where(e => e.TypeId == 8)
                    .ToList()
                    .ForEach(async door =>
                    {
                        await requester.Send("zigbee2mqtt/" + door.Identifier + "/activate", new SmartLockUnlock()
                        {

                        });
                    });
            }
        }
    }

    public class RFID
    {
        public string rfid { get; set; }

        public RFID() { }
    }

    public class SmartLockUnlock
    {
        public int duration { get; set; } = 2000;
    }
}

