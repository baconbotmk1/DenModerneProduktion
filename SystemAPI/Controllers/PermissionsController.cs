using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using Shared.Models;
using Shared.Services;

namespace SystemAPI.Controllers
{
    [ApiController]
    [Route("api/permissions")]
    public class PermissionsController : GenericCRUDController<Permission, CreatePermissionDTO>
    {
        public PermissionsController(DataContext Context, GenericRepository<Permission>? DIrepository = null) : base(Context, DIrepository)
        {
        }
    }
}

