using System;
namespace Shared.Responses
{
	public class AcceptedResponse<T> : BaseResponse
    {
		public T Data { get; set; }

		public AcceptedResponse( T data, int statusCode = 200 )
		{
			Status = statusCode;

			Data = data;
		}
    }
}

