using System;
using AutoMapper;
using Shared.DTOs;
using Shared.Models;

namespace Shared.MappingProfiles
{
	public class UserMapping : Profile
	{
		public UserMapping()
		{
			CreateMap<UserDTO, User>();
		}
	}
}

