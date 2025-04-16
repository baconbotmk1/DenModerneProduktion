using System;
using Shared.Models;

namespace Shared.Responses
{
	public abstract class BaseResponse
	{
        public int Status { get; set; }

        public int StatusCode => Status;

        public string Message { get; set; } = "";

        public bool IsSuccessResponse => Status >= 200 && Status < 300;

        public T? TryGetData<T>() where T : class
        {
            return this is AcceptedResponse<T> ? ((AcceptedResponse<T>)this).Data : null;
        }
    }
}

