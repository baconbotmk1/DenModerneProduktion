using System;
using Shared.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace SystemAPI.SwaggerExamples
{
	public class CreateUserExample : IExamplesProvider<CreateUser>
    {
        public CreateUser GetExamples()
        {
            return new CreateUser
            {
                Name = "John Doe",
                IsActive = true
            };
        }
    }
}

