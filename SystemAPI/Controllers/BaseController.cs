using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared;
using Shared.Models;
using System.Diagnostics;
using System.Text.Json;

namespace SystemAPI.Controllers
{
    public abstract class BaseController<T> : Controller where T : class
    {
        private readonly bool ShowDebug;
        public BaseController( IConfiguration configuration)
        {
            ShowDebug = configuration.GetValue<bool>("ShowDebug");
        }

        protected DataContext context;
        protected IRepository<T> repository;

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

        public BaseController(DataContext Context, IRepository<T> DIrepository)
        {
            context = Context;
            repository = DIrepository;
        }

        protected override void Dispose(bool disposing)
        {
            repository.Dispose();
            base.Dispose(disposing);
        }
    }
}
