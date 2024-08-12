namespace Ecom.API.Helper
{
    public class ResponseAPI
    {
        public ResponseAPI(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFormStatusCode(StatusCode);
        }
        private string GetMessageFormStatusCode(int statuscode)
        {
            return statuscode switch
            {
                200 => "Done",
                400 => "Bad Request",
                401 => "Un Authorized",
                500 => "server Error",
                _ => null,
            };
        }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
