using System.Net;

namespace Application.DTO.Response
{
    public class ErrorResponse : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; } = DateTime.Now;

        public ErrorResponse(HttpStatusCode statusCode, string message) 
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
