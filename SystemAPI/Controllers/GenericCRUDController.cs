using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class GenericCRUDController<T,T2> : Controller where T : class where T2 : class
    {
        private GenericRepository<T> repository;
        private readonly IMapper _mapper;

        public GenericCRUDController(IMapper mapper, GenericRepository<T>? DIrepository = null)
        {
            repository = DIrepository ?? new GenericRepository<T>(new Shared.DataContext());
            _mapper = mapper;
        }


        [HttpGet]
        public IEnumerable<T> Get()
        {
            return repository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<T> GetUserById(int id)
        {
            T? securityGroup = repository.GetById(id);

            if(securityGroup == null)
            {
                return NotFound();
            }

            return Ok(securityGroup);
        }


        [HttpPost]
        public void Post([FromBody]T2 data)
        {
            T item = _mapper.Map<T>(data);

            repository.Insert(item);
            repository.Save();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]T2 data)
        {
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
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

