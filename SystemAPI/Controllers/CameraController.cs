using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/camera")]
    public class StreamController : Controller
    {
        private readonly DataContext _context;
        private int? _typeId = null;
        private int? _linkTypeId = null;

        public StreamController( DataContext context )
        {
            _context = context;
        }

        [HttpGet("live/{device_id}")]
        public async Task GetLiveDevice( int device_id )
        {
            if(_typeId == null)
            {
                _typeId = _context.DeviceTypes.AsQueryable().First(e => e.Name == "Kamera").Id;
            }

            if (_typeId == null)
            {
                _linkTypeId = _context.DeviceInfoTypes.AsQueryable().First(e => e.Name == "RTSP Link").Id;
            }

            Device? cameraObj = _context.Devices
                .AsQueryable()
                .Where(e => e.TypeId == _typeId)
                .Where(e => e.Id == device_id)
                .Include(e => e.Infos)
                .FirstOrDefault();

            if(cameraObj == null)
            {
                return;
            }

            DeviceInfo? linkObj = cameraObj.Infos.Where(e => e.TypeId == _linkTypeId).FirstOrDefault();
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

