using System;
namespace Shared.Responses
{
	public class ErrorResponse : BaseResponse
	{
        public string Title { get; set; } = "";
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();

        public ErrorResponse()
		{
			Status = 500;
		}

        public ErrorResponse( int status = 500, string message = "" )
        {
            Status = status;
            Message = message;
            Title = message;
        }
    }
}

