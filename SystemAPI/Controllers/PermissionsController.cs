using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Models;
using Shared.Services;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/permissions")]
    public class PermissionsController : GenericCRUDController<Permission,PermissionDTO>
    {
        public PermissionsController(IMapper mapper) : base(mapper)
        {
        }
    }
}

