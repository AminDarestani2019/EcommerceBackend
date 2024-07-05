namespace Domain.Exceptions
{
    public class ApiToReturn
    {
        public ApiToReturn()
        {
                
        }
        public ApiToReturn(string message)
        {
            Message = message;
            Messages.Add(message);
        }
        public ApiToReturn(int statusCode, string message)
        { 
            StatusCode = statusCode;
            Message = message;
            Messages.Add(message);
        }
        public ApiToReturn(int statusCode, List<string> messages)
        {
            StatusCode = statusCode;
            Messages = messages;
        }
        public ApiToReturn(int statusCode,List<string> messages, string detail)
        {
            StatusCode=statusCode;
            Detail = detail;
            Messages = messages;
        }
        public ApiToReturn(int statusCode, string message, string detail)
        {
            StatusCode = statusCode;
            Detail = detail;
            Message = message;
            Messages.Add(message);  
        }
        public ApiToReturn(int statusCode,string message, List<string> messages, string detail)
        {
            StatusCode = statusCode;
            Detail = detail;
            Messages = messages;
            Message = message;
        }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string Detail {  get; set; }
        public List<string> Messages { get; set; } = new();
    }
}
