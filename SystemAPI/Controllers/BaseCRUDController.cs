using System.Diagnostics;

namespace SystemAPI.Controllers
{
    public abstract class BaseCRUDController<T> : BaseController where T : class
    {
        public BaseCRUDController( DataContext Context, IRepository<T> DIrepository) : base(Context)
        {
            repository = DIrepository;
        }

        protected IRepository<T> repository;

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult HandleError(Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.GetType().Name);
            Debug.WriteLine(ex.GetType().Namespace);

            return StatusCode(500, ex.Message);

            /*if (ShowDebug)
            {
                return StatusCode(500, ex.Message);
            }
            else
            {
                return StatusCode(500, "Internal Error");
            }*/
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<T> HandleException(Func<ActionResult<T>> func)
        {
            try
            {
                return func();
            }
            catch(Exception ex)
            {
                return HandleError(ex);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult HandleExceptions(Func<ActionResult> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            repository.Dispose();

            base.Dispose(disposing);
        }
    }
}
