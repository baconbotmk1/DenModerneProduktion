using MQTTnet;
using System.Text;
using System.Text.Json;

namespace SystemAPI.Services
{
    public class MqttRecorderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private IMqttClient _mqttClient;


        public MqttRecorderService(IServiceScopeFactory scopeFactory, IConfiguration configuration )
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new MqttClientFactory();
            _mqttClient = factory.CreateMqttClient();

            IConfigurationSection mqttSection = _configuration.GetSection("Mqtt");

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(mqttSection.GetValue<string>("Host"), mqttSection.GetValue<int>("Port"))
                .WithClientId(mqttSection.GetValue<string>("ClientId"))
                .Build();

            _mqttClient.ConnectedAsync += async (MqttClientConnectedEventArgs e) =>
            {
                Console.WriteLine("Connected to MQTT Broker.");
                await _mqttClient.SubscribeAsync("#");
            };

            _mqttClient.ApplicationMessageReceivedAsync += async (MqttApplicationMessageReceivedEventArgs e) =>
            {
                var topic = e.ApplicationMessage.Topic;
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                //Console.WriteLine($"Received: {topic} - {payload}");

                string identifier = topic;

                if(identifier.Contains("zigbee2mqtt"))
                {
                    identifier = identifier.Substring(identifier.LastIndexOf('/') + 1);

                    if(!identifier.StartsWith("0x"))
                    {
                        return;
                    }

                    Console.WriteLine($"Received: {topic} - {payload}");
                    Console.WriteLine($"ZIgbee2MQTT - {identifier}");

                    using var scope = _scopeFactory.CreateScope();
                    using (var db = scope.ServiceProvider.GetRequiredService<DataContext>())
                    {
                        Device? device = db.Devices.FirstOrDefault(e => e.Identifier != null && e.Identifier.ToLower() == identifier.ToLower());
                        if(device != null)
                        {
                            List<DeviceMqttMap> possibleMaps = db.DeviceMqttMaps
                                .AsQueryable()
                                .Where(e => e.DeviceTypeId == device.TypeId)
                                .ToList();

                            Dictionary<string, object> fields = JsonSerializer.Deserialize<Dictionary<string, object>>(payload) ?? new Dictionary<string, object>();

                            foreach(KeyValuePair<string, object> keyValuePair in fields)
                            {
                                if(possibleMaps.Any(e => e.FieldName.ToLower() == keyValuePair.Key.ToLower()))
                                {
                                    DeviceMqttMap map = possibleMaps.First(e => e.FieldName.ToLower() == keyValuePair.Key.ToLower());

                                    if(map.InfoTypeId != null && map.InfoTypeId > 0)
                                    {
                                        DeviceInfo? info = db.DeviceInfos
                                            .AsQueryable()
                                            .Where(e => e.DeviceId == device.Id && e.TypeId == map.InfoTypeId)
                                            .FirstOrDefault();

                                        if (info != null)
                                        {
                                            info.Value = keyValuePair.Value.ToString().ToLower();
                                            db.DeviceInfos.Update(info);
                                            db.SaveChanges();
                                        }
                                    }

                                    if(map.DataTypeId != null && map.DataTypeId > 0)
                                    {
                                        db.DeviceDatas.Add(new DeviceData()
                                        {
                                            DeviceId = device.Id,
                                            Timestamp = DateTime.UtcNow,
                                            TypeId = map.DataTypeId.Value,
                                            Value = keyValuePair.Value.ToString().ToLower(),
                                        });
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }

                }
            };

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
    }
}
