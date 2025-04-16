using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Models;
using Shared.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class GenericCRUDController<T,T2> : Controller where T : class where T2 : class
    {
        protected DataContext context;
        protected GenericRepository<T> repository;

        public GenericCRUDController(DataContext Context, GenericRepository<T>? DIrepository = null)
        {
            context = Context;
            repository = DIrepository ?? new GenericRepository<T>(new Shared.DataContext());
        }


        [HttpGet]
        public virtual ActionResult<IEnumerable<T>> Get()
        {
            IEnumerable<T> data;

            try
            {
                data = repository.Get();
            }
            catch(Exception ex)
            {
                return StatusCode(500, "internal error");
            }

            return Ok(data);
        }

        [HttpGet("{id}")]
        public virtual ActionResult<T> GetUserById(int id)
        {
            T? securityGroup = repository.GetById(id);

            if(securityGroup == null)
            {
                return NotFound();
            }

            return Ok(securityGroup);
        }


        [HttpPost]
        public virtual ActionResult<T> Post([FromBody]T2 data)
        {
            T item = data.Adapt<T>();

            repository.Insert(item);
            repository.Save();

            return Ok(item);
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        public virtual ActionResult<T> Put(int id, [FromBody]T2 body)
        {
            T? item = repository.GetById(id);

            if (item == null)
            {
                return NotFound();
            }

            body.Adapt(item);

            repository.Update(item);
            repository.Save();

            return Ok(item);
        }

        [HttpDelete("{id}")]
        public virtual ActionResult Delete(int id)
        {
            T? foundObject = repository.GetById(id);

            if (foundObject == null)
            {
                return NotFound();
            }

            bool deleted = repository.Delete(id);
            repository.Save();

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            repository.Dispose();
            base.Dispose(disposing);
        }
    }
}

