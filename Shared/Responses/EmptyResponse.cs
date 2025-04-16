using System;
namespace Shared.Responses
{
	public class EmptyResponse : BaseResponse
    {
		public EmptyResponse( int status = 200 )
		{
			Status = status;
		}
    }
}

