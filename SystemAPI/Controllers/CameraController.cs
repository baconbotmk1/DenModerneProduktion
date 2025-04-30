using System.Diagnostics;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/camera")]
    public class StreamController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider) : BaseController(_context, _configuration, _provider)
    {
        private int? typeId = null;
        private int? linkTypeId = null;


        /// <summary>
        /// Creates a stream from a camera device to be stream to a browser
        /// </summary>
        /// <remarks>Still a work-in-progress</remarks>
        /// <param name="device_id"></param>
        /// <returns></returns>
        [HttpGet("live/{device_id}")]
        public async Task GetLiveDevice( int device_id )
        {
            if(typeId == null)
            {
                typeId = context.DeviceTypes.AsQueryable().First(e => e.Name == "Kamera").Id;
            }

            if (typeId == null)
            {
                linkTypeId = context.DeviceInfoTypes.AsQueryable().First(e => e.Name == "RTSP Link").Id;
            }

            Device? cameraObj = context.Devices
                .AsQueryable()
                .Where(e => e.TypeId == typeId)
                .Where(e => e.Id == device_id)
                .Include(e => e.Infos)
                .FirstOrDefault();

            if(cameraObj == null)
            {
                return;
            }

            DeviceInfo? linkObj = cameraObj.Infos.Where(e => e.TypeId == linkTypeId).FirstOrDefault();
            if(linkObj == null)
            {
                return;
            }

            string link = linkObj.Value;

            Response.Headers.Add("Content-Type", "video/mp2t");

            using var ffmpeg = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-i {link} -f mpegts -codec:v mpeg1video -an -",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            ffmpeg.Start();

            await ffmpeg.StandardOutput.BaseStream.CopyToAsync(Response.Body);

            await ffmpeg.WaitForExitAsync();
        }
    }

}

