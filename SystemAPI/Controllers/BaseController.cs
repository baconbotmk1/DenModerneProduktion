using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared;
using Shared.Models;

namespace SystemAPI.Controllers
{
    public abstract class BaseController<T> : Controller where T : class
    {
        protected DataContext context;
        protected IRepository<T> repository;

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
