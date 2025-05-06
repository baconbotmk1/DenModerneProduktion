using System.Diagnostics;

namespace SystemAPI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly IConfiguration configuration;
        protected readonly IServiceProvider provider;
        protected readonly DataContext context;

        public BaseController(DataContext _context, IConfiguration _configuration, IServiceProvider _provider)
        {
            context = _context;
            configuration = _configuration;
            provider = _provider;
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}
