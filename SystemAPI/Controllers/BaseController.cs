using System.Diagnostics;

namespace SystemAPI.Controllers
{
    public abstract class BaseController : Controller
    {
        public BaseController( DataContext Context)
        {
            context = Context;
        }

        protected DataContext context;

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}
