using System;
using Shared.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace SystemAPI.SwaggerExamples
{
	public class CreateSectionExample : IExamplesProvider<CreateSection>
    {
        public CreateSection GetExamples()
        {
            return new CreateSection
            {
                Name = "Example section",
                BuildingId = 1
            };
        }
    }
}

