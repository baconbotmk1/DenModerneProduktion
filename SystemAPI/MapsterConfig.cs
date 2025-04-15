using System;
using Mapster;
using Shared.DTOs;
using Shared.Models;

namespace SystemAPI
{
	public static class MapsterConfig
	{
        public static void RegisterMappings()
        {
            TypeAdapterConfig.GlobalSettings.NewConfig<GetUserDTO, User>()
                .Map(dest => dest.SecurityGroups,
                     src => src.UserSecurityGroups.Select(usg => usg.SecurityGroup));

            TypeAdapterConfig.GlobalSettings.NewConfig<User, GetUserDTO>()
                .Map(dest => dest.SecurityGroups,
                     src => src.UserSecurityGroups.Select(usg => usg.SecurityGroup));

            TypeAdapterConfig.GlobalSettings.NewConfig<SecurityGroup, GetUserSecurityGroupDTO>()
                .Map(dest => dest.Users,
                     src => src.UserSecurityGroups.Select(usg => usg.User));
        }
    }
}

