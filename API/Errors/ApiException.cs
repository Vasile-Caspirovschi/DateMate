namespace API.Errors
{
    public class ApiException
    {
        public ApiException(int statusCode, string message = null!, string details = null!)
        {
            StatusCode = statusCode;
            Message = message;
            Detailes = details;
        }

        public int StatusCode { get; set; }
        public string  Message { get; set; }
        public string Detailes { get; set; }
    }
}
