using System;
namespace Shared.Responses
{
	public class AcceptedResponse<T> : BaseResponse
    {
		public T Data { get; set; }

		public AcceptedResponse( T data )
		{
			Status = 200;

			Data = data;
		}
    }
}

