using System;
namespace Shared.Responses
{
	public class ServerErrorResponse : ErrorResponse
	{
        public ServerErrorResponse()
		{
			Status = 500;
		}
	}
}

