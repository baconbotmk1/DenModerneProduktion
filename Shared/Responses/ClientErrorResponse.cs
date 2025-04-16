using System;
namespace Shared.Responses
{
	public class ClientErrorResponse : ErrorResponse
	{
        public ClientErrorResponse()
		{
			Status = 400;
		}
	}
}

