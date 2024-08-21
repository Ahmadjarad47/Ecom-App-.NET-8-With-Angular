namespace Ecom.API.Helper
{
    public class ApiExceptions : ResponseAPI
    {
        public ApiExceptions(int statusCode, string message = null,string details=null) : base(statusCode, message)
        {
            Details = details;
        }
        public string Details { get; set; }
    }
}
