using System;

namespace Shared.Responses
{
    public class ApiValidationError : ClientErrorResponse
    {
        public ApiValidationError()
        {
            Status = 403;
        }
    }
}

