using System.Globalization;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MQTTnet;
using Shared;
using Shared.Models;

namespace MqttHandler;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private IMqttClient _mqttClient;
    private bool? _canSaveUnknown;

    private DateTime? _lastDataUpdate;

    private Dictionary<int,SavedDataTypeLimit> _savedDataTypeLimits;
    private List<DeviceMqttMap> _savedDeviceMqttMaps;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _logger = logger;
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
            await _mqttClient.SubscribeAsync("#");
        };

        _mqttClient.ApplicationMessageReceivedAsync += async (MqttApplicationMessageReceivedEventArgs e) =>
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

            Console.WriteLine($"\n[MQTT-BGS] Got message");

            string identifier = topic;

            if(_lastDataUpdate == null || (DateTime.UtcNow - _lastDataUpdate) >= TimeSpan.FromMinutes(10))
            {
                await UpdateData();
            }

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
        _lastDataUpdate = DateTime.UtcNow;

        Console.WriteLine("\n[MQTT-BGS] Updating data...");

        using var scope = _scopeFactory.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        _savedDataTypeLimits = context.DeviceDataTypes
            .AsQueryable()
            .Include(e => e.LimitValues)
            .ToList()
            .Where(e => e.LimitValues.Count > 0)
            .Select(e =>
            {
                DeviceDataLimitValue closest = e.LimitValues.FirstOrDefault(e2 => e2.RoomId != null)
                    ?? e.LimitValues.FirstOrDefault(e2 => e2.SectionId != null)
                    ?? e.LimitValues.FirstOrDefault(e2 => e2.BuildingId != null)
                    ?? e.LimitValues.First(e2 => e2.CadastreId != null);

                return new SavedDataTypeLimit()
                {
                    Id = e.Id,
                    Maximum = closest.MaximumLimit != null ? closest.MaximumLimit : null,
                    Minimum = closest.MinimumLimit != null ? closest.MinimumLimit : null,
                    RefId = closest.RoomId ?? closest.SectionId ?? closest.BuildingId ?? closest.CadastreId ?? 0,
                    RefType = closest.RoomId != null ? "room" : (closest.SectionId != null ? "section" : (closest.BuildingId != null ? "building" : "cadastre")),
                };
            })
            .ToDictionary(e => e.Id);

        _savedDeviceMqttMaps = context.DeviceMqttMaps.AsQueryable().ToList();

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
        
        Device? device = db.Devices.FirstOrDefault(e => e.Identifier != null && e.Identifier.ToLower() == identifier.ToLower());
        if (device == null)
        {
            if(_canSaveUnknown == null)
            {
                _canSaveUnknown = CheckCanSaveUnknown();
            }

            Console.WriteLine($"[MQTT-BGS] Unknown Zigbee2MQTT ({(_canSaveUnknown.Value ? "Reported" : "Can't report")}) - {identifier} - {payload}");

            if(_canSaveUnknown.Value)
            {
                var checkIfExists = db.UnknownMqttDevices
                    .AsQueryable()
                    .FirstOrDefault(e => e.Identifier.ToLower() == identifier.ToLower());

                if(checkIfExists != null)
                {
                    checkIfExists.LastFoundAt = currentTime;
                    checkIfExists.LastPayload = payload;
                    await db.SaveChangesAsync();
                    return;
                }

                db.UnknownMqttDevices.Add(new UnknownMqttDevices()
                {
                    Identifier = identifier,
                    FirstFoundAt = currentTime,
                    LastFoundAt = currentTime,
                    LastPayload = payload
                });
                await db.SaveChangesAsync();
            }

            return;
        }

        Console.WriteLine($"[MQTT-BGS] Known Zigbee2MQTT - {identifier} - {payload}");

        List<DeviceMqttMap> possibleMaps = _savedDeviceMqttMaps
            .Where(e => e.DeviceTypeId == device.TypeId)
            .ToList();

        Dictionary<string, object> fields = JsonSerializer.Deserialize<Dictionary<string, object>>(payload) ?? new Dictionary<string, object>();

        foreach (KeyValuePair<string, object> keyValuePair in fields.Where(keyValuePair => possibleMaps.Any(e => e.FieldName.ToLower() == keyValuePair.Key.ToLower())))
        {
            DeviceMqttMap map = possibleMaps.First(e => e.FieldName.ToLower() == keyValuePair.Key.ToLower());

            if (map.InfoTypeId != null && map.InfoTypeId > 0)
            {
                DeviceInfo? info = db.DeviceInfos
                    .AsQueryable()
                    .Where(e => e.DeviceId == device.Id && e.TypeId == map.InfoTypeId)
                    .FirstOrDefault();

                if (info != null)
                {
                    info.Value = keyValuePair.Value.ToString().ToLower();
                    info.Timestamp = DateTime.UtcNow;
                    db.DeviceInfos.Update(info);
                    await db.SaveChangesAsync();
                }
                else
                {
                    db.DeviceInfos.Add(new DeviceInfo()
                    {
                        DeviceId = device.Id,
                        Timestamp = DateTime.UtcNow,
                        TypeId = map.InfoTypeId.Value,
                        Value = keyValuePair.Value.ToString().ToLower(),

                    });
                    await db.SaveChangesAsync();
                }
            }

            if (map.DataTypeId != null && map.DataTypeId > 0)
            {
                var newDeviceData = new DeviceData()
                {
                    DeviceId = device.Id,
                    Timestamp = DateTime.UtcNow,
                    TypeId = map.DataTypeId.Value,
                    Value = keyValuePair.Value.ToString().ToLower(),
                };
                db.DeviceDatas.Add(newDeviceData);
                await db.SaveChangesAsync();

                if (_savedDataTypeLimits.ContainsKey(map.DataTypeId.Value) && double.TryParse(keyValuePair.Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                {
                    double? min = _savedDataTypeLimits[map.DataTypeId.Value].Minimum;
                    double? max = _savedDataTypeLimits[map.DataTypeId.Value].Maximum;

                    if(min != null && value <= min.Value)
                    {
                        db.DeviceEvents.Add(new DeviceEvent()
                        {
                            DeviceId = device.Id,
                            Timestamp = DateTime.UtcNow,
                            TypeId = 1,
                            DeviceDataId = newDeviceData.Id,
                        });

                        await db.SaveChangesAsync();
                    }

                    if (max != null && value >= max.Value)
                    {
                        db.DeviceEvents.Add(new DeviceEvent()
                        {
                            DeviceId = device.Id,
                            Timestamp = DateTime.UtcNow,
                            TypeId = 2,
                            DeviceDataId = newDeviceData.Id,
                        });

                        await db.SaveChangesAsync();
                    }

                }
            }
        }
    }
}

