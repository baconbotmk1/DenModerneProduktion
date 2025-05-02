using MQTTnet;
using System.Text.Json;

namespace SystemAPI.Services
{
    /// <summary>
    /// Class to send MQTT messages.
    /// </summary>
    public class MqttRequester
    {
        private readonly IConfiguration _configuration;

        public MqttRequester(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Send a message to the MQTT broker.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="data"></param>
        /// <param name="retain"></param>
        /// <returns></returns>
        async public Task Send( string topic, object data, bool retain = false )
        {
            var factory = new MqttClientFactory();
            var mqttClient = factory.CreateMqttClient();
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

            Console.WriteLine("[MQTT-BGS] Connecting to broker...");

            await mqttClient.ConnectAsync(options, CancellationToken.None);

            Console.WriteLine("[MQTT-BGS] Connected to broker!");

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(JsonSerializer.Serialize(data))
                .WithRetainFlag(retain)
                .Build();

            Console.WriteLine("[MQTT-BGS] Sending message...");

            await mqttClient.PublishAsync(message, CancellationToken.None);

            Console.WriteLine("[MQTT-BGS] Message sent!");

            Console.WriteLine("[MQTT-BGS] Disconnecting from broker...");

            await mqttClient.DisconnectAsync();

            Console.WriteLine("[MQTT-BGS] Disconnected from broker!");

            mqttClient.Dispose();

            Console.WriteLine("[MQTT-BGS] Disposed of client!");

            Console.WriteLine("[MQTT-BGS] Done!");
        }
    }
}
