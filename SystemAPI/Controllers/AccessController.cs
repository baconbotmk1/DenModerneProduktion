using SystemAPI.Services;

namespace SystemAPI.Controllers
{
    /// <summary>
    /// Controller for controlling access to rooms and sections
    /// </summary>
    /// <param name="_context"></param>
    /// <param name="_configuration"></param>
    /// <param name="_provider"></param>
    /// <param name="_mqttRequester"></param>
    [ApiController]
    [Route("api/access")]
    public class AccessController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider, MqttRequester _mqttRequester) : BaseController(_context, _configuration, _provider)
    {
        private readonly MqttRequester mqttRequester = _mqttRequester;

        /// <summary>
        /// Remote unlocking of a door
        /// </summary>
        /// <param name="device_id"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        [HttpPost("remote_unlock/{device_id}")]
        public ActionResult TryLogin(int device_id, int duration = 2000)
        {
            Device? device = context.Devices.FirstOrDefault(e => e.Id == device_id);

            if (device == null)
            {
                return NotFound();
            }


            return Ok();
        }
    }
}

