using System;
using AutoMapper;
using Shared.DTOs;
using Shared.Models;

namespace Shared.MappingProfiles
{
	public class PermissionMapping : Profile
	{
		public PermissionMapping()
		{
			CreateMap<PermissionDTO, Permission>();
		}
	}
}

