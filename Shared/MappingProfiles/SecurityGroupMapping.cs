using System;
using AutoMapper;
using Shared.DTOs;
using Shared.Models;

namespace Shared.MappingProfiles
{
	public class SecurityGroupMapping : Profile
	{
		public SecurityGroupMapping()
		{
			CreateMap<SecurityGroupDTO, SecurityGroup>();
		}
	}
}

