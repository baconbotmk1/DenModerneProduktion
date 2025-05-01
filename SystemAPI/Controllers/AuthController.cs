using Shared.DTOs.Auth;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using MQTTnet;
using SystemAPI.Helpers;
using SystemAPI.Services;

namespace SystemAPI.Controllers
{
    /// <summary>
    /// Controller for authentication
    /// </summary>
    /// <param name="_context"></param>
    /// <param name="_configuration"></param>
    /// <param name="_provider"></param>
    [ApiController]
    [Route("api/auth")]
    public class AuthController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        /// <summary>
        /// Try to login with username and password
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Request a password reset
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("reset_password")]
        public ActionResult Post([FromBody] StartPasswordResetPost data)
        {
            User? user = context.Users.FirstOrDefault(e => e.Username.ToLower() == data.username.ToLower());
            if (user == null)
            {
                return NotFound();
            }

            string token = CryptoHelper.GenerateSaltString();

            user.ResetToken = token;

            MailService mailService = provider.GetRequiredService<MailService>();

            mailService.SendResetLink(user.Username, token);

            context.Attach(user);
            context.SaveChanges();

            return Ok(new object { });
        }

        /// <summary>
        /// Confirm the password reset
        /// </summary>
        /// <param name="token"></param>
        /// <param name="data"></param>
        /// <returns></returns>
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

            MailService mailService = provider.GetRequiredService<MailService>();

            mailService.SendConfirmReset(user.Username);

            return Ok(new object { });
        }

        /// <summary>
        /// Request the fingerprint scanner to start scanning
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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
                var config = provider.GetService<IConfiguration>()!;

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

        /// <summary>
        /// Receive image data from the fingerprint scanner
        /// </summary>
        /// <returns></returns>
        [HttpPost("fingerprint")]
        public async Task<IActionResult> ReceiveData()
        {
            using (var memoryStream = new MemoryStream())
            {
                await Request.Body.CopyToAsync(memoryStream);

                byte[] receivedData = memoryStream.ToArray();
                int width = 256;
                int height = 288;

                var image = CreateImageFromRawRgb(receivedData, width, height);

                using (StreamWriter writer = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DenModerneProduktion", $"UploadedData_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-f")}.bmp")))
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

