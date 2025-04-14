using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Services;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/security_group")]
    public class SecurityGroupController : Controller
    {
        private GenericRepository<SecurityGroup> repository;

        public SecurityGroupController(GenericRepository<SecurityGroup>? securityGroupRepository = null)
        {
            repository = securityGroupRepository ?? new GenericRepository<SecurityGroup>(new Shared.DataContext());
        }


        [HttpGet]
        public IEnumerable<SecurityGroup> Get()
        {
            return repository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<SecurityGroup> GetUserById(int id)
        {
            SecurityGroup? securityGroup = repository.GetById(id);

            if(securityGroup == null)
            {
                return NotFound();
            }

            return Ok(securityGroup);
        }


        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            SecurityGroup? securityGroup = repository.GetById(id);

            if (securityGroup == null)
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

