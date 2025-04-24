using Microsoft.OpenApi.Models;
using MQTTnet;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace SystemAPI.Services
{
    public class MainSenderService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private IMqttClient _mqttClient;


        public MainSenderService(IServiceScopeFactory scopeFactory, IConfiguration configuration )
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }


        async public Task SendEmail( string toEmail, string subject, string html = "", string text = "" )
        {
            using var scope = _scopeFactory.CreateAsyncScope();

            using var httpClient = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("mailer");

            IConfigurationSection mailerSection = _configuration.GetSection("Mailer");

            var data = new EmailWithHtml()
            {
                from = new EmailWithHtml.Address()
                {
                    email = mailerSection.GetValue<string>("FromEmail") ?? "missing@sender.dk",
                    name = mailerSection.GetValue<string>("FromName") ?? "",
                },
                to = new List<EmailWithHtml.Address>()
                {
                    new EmailWithHtml.Address()
                    {
                       email = toEmail 
                    }
                },
                subject = subject,
                html = html,
                text = text
            };

            await httpClient.PostAsJsonAsync("https://sandbox.api.mailtrap.io/api/send/2479650", data);
        }

        public class EmailWithHtml
        {
            public required Address from { get; set; }
            public required List<Address> to { get; set; }
            public required string subject { get; set; }

            public required string html { get; set; }
            public string text { get; set; }

            public class Address
            {
                public required string email { get; set; }
                public string name { get; set; } = "";
            }
        }
    }
}
