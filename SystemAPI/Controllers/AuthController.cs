using Microsoft.AspNetCore.Identity.Data;
using Shared.DTOs.AccessCard;
using Shared.DTOs.Auth;
using Shared.Migrations;
using Shared.Models;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using SystemAPI.Helpers;
using static System.Net.Mime.MediaTypeNames;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using MQTTnet;
using Microsoft.Extensions.Configuration;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _provider;
        public AuthController(DataContext Context, IConfiguration configuration, IServiceProvider provider) : base(Context)
        {
            _configuration = configuration;
            _provider = provider;
        }


        [HttpPost("login")]
        public ActionResult<LoginResult> TryLogin([FromBody] LoginPost data)
        {
            User? user = context.Users
                .AsQueryable()
                .Include(e => e.UserSecurityGroups)
                    .ThenInclude(e2 => e2.SecurityGroup)
                        .ThenInclude(e3 => e3.SecurityGroupPermissions)
                            .ThenInclude(e4 => e4.Permission)
                .FirstOrDefault(e => e.Username.ToLower() == data.username.ToLower());

            if (user == null)
            {
                return NotFound();
            }

            if (user.HashedPassword == null || user.Salt == null)
            {
                return NotFound();
            }

            byte[] hashedPasswordBytes = Convert.FromBase64String(user.HashedPassword);
            byte[] saltBytes = Convert.FromBase64String(user.Salt);

            if (!CryptoHelper.ComparePasswordWithHash(data.password, hashedPasswordBytes, saltBytes))
            {
                return NotFound();
            }

            List<Permission> permissions = user.UserSecurityGroups.Select(e => e.SecurityGroup).SelectMany(e => e.SecurityGroupPermissions.Select(e2 => e2.Permission))
                    .Distinct()
                    .ToList();

            return Ok(new LoginResult(user, permissions));
        }


        [HttpPost("reset_password")]
        public ActionResult Post([FromBody] StartPasswordResetPost data)
        {
            User? user = context.Users.FirstOrDefault(e => e.Username.ToLower() == data.username.ToLower());
            if (user == null)
            {
                return NotFound();
            }

            string token = CryptoHelper.GenerateSaltString();
            string state = CryptoHelper.GenerateSaltString(); //Delete?

            user.ResetToken = token;

            IConfigurationSection hostingSection = _configuration.GetSection("Hosting");
            var url = hostingSection.GetValue<string>("Url");
            var port = hostingSection.GetValue<int?>("Port");
            var completeUrl = url + (port != null ? $":{port}" : "") + "/forgotpassword/" + token;


            IConfigurationSection emailSection = _configuration.GetSection("Email");
            var mail = emailSection.GetValue<string>("Mail");
            var password = emailSection.GetValue<string>("Password");
            var smtp = emailSection.GetValue<string>("Smtp");
            var smtpPort = emailSection.GetValue<int>("Port");
            var useSSL = emailSection.GetValue<bool>("UseSSL");

            MailHelper.SendResetLink(mail, user.Username, smtp, smtpPort, completeUrl, password, useSSL);

            context.Attach(user);
            context.SaveChanges();

            return Ok(new object { });
        }


        [HttpPost("reset_password/{token}")]
        public ActionResult Post(string token, [FromBody] ConfirmPasswordResetPost data)
        {
            User? user = context.Users.FirstOrDefault(e => e.ResetToken == token);
            if (user == null)
            {
                return NotFound();
            }
            byte[] salt = user.Salt != null ? Convert.FromBase64String(user.Salt) : CryptoHelper.GenerateSalt();

            if (user.Salt == null)
            {
                user.Salt = Convert.ToBase64String(salt);
            }
            user.ResetToken = null;
            user.HashedPassword = Convert.ToBase64String(CryptoHelper.HashPassword(data.password, salt));


            context.Attach(user);
            context.SaveChanges();

            IConfigurationSection emailSection = _configuration.GetSection("Email");
            var mail = emailSection.GetValue<string>("Mail");
            var password = emailSection.GetValue<string>("Password");
            var smtp = emailSection.GetValue<string>("Smtp");
            var smtpPort = emailSection.GetValue<int>("Port");
            var useSSL = emailSection.GetValue<bool>("UseSSL");

            MailHelper.SendConfirmReset(mail, user.Username, smtp, smtpPort, password, useSSL);

            return Ok(new object { });
        }

        public static int counter = 0;

        [HttpPost("fingerprint/start")]
        public async Task<IActionResult> StartFingerprint( [FromBody] StartFingerprint request )
        {
            User? user = context.Users
                .AsQueryable()
                .FirstOrDefault(e => e.Id == request.UserId);

            if(user == null)
            {
                return NotFound();
            }

            MqttClientFactory factory = new MqttClientFactory();

            using (var _mqttClient = factory.CreateMqttClient())
            {
                var config = _provider.GetService<IConfiguration>()!;

                IConfigurationSection mqttSection = config.GetSection("Mqtt");

                Console.WriteLine("[MQTT-BGS] Building settings...");

                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(mqttSection.GetValue<string>("Host"), mqttSection.GetValue<int>("Port"))
                    .WithTlsOptions(new MqttClientTlsOptions()
                    {
                        UseTls = true
                    })
                    .WithCredentials(mqttSection.GetValue<string>("Username"), mqttSection.GetValue<string>("Password"))
                    .Build();

                await _mqttClient.ConnectAsync(options, CancellationToken.None);

                var data = (new MqttApplicationMessageBuilder())
                    .WithTopic("zigbee2mqtt/" + request.Identifier + "/activate")
                    .WithPayload($"{{\"user_id\": { request.UserId }}}")
                    .Build();

                await _mqttClient.PublishAsync(data, CancellationToken.None);

                await _mqttClient.DisconnectAsync();
            }

            return Ok();
        }

        [HttpPost("fingerprint")]
        public async Task<IActionResult> ReceiveData()
        {
            using (var memoryStream = new MemoryStream())
            {
                await Request.Body.CopyToAsync(memoryStream);

                byte[] receivedData = memoryStream.ToArray();
                int width = 256;
                int height = 288;

                var image = CreateImageFromRawRgb(receivedData, 256, 288);

                using (StreamWriter writer = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DenModerneProduktion", $"UploadedData_{counter++}.bmp")))
                {
                    image.Save(writer.BaseStream,new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder());
                }

                return Ok();
            }
        }

        static Image<L8> CreateImageFromRawRgb(byte[] rawData, int width, int height)
        {
            if (rawData.Length != (width * height) / 2)
                throw new ArgumentException("Invalid raw data size for 4-bit packed image.");

            Image<L8> image = new Image<L8>(width, height);

            int idx = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x += 2)
                {
                    byte b = rawData[idx++];

                    byte highNibble = (byte)(b >> 4);
                    byte lowNibble = (byte)(b & 0x0F);

                    image[x, y] = new L8((byte)(highNibble * 17));
                    if (x + 1 < width)
                        image[x + 1, y] = new L8((byte)(lowNibble * 17));
                }
            }

            return image;
        }
    }
}

