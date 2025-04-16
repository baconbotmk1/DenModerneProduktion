using System;
namespace Shared.Responses
{
	public abstract class BaseResponse
	{
        public int Status { get; set; }

        public int StatusCode => Status;

        public string Message { get; set; } = "";

        public bool IsSuccessResponse => Status >= 200 && Status < 300;
    }
}

