
namespace ExamEdrian.Services
{
    public class ErrorEventArgs
    {
        public string Message { get; }
        public ErrorType ErrorCode { get; }

        public ErrorEventArgs(ErrorType errorCode, string message = null)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }

    public enum ErrorType
    {
        Unkown,
        Timeout,
        Unauthorized,
        BadRequest,
        ServerError
    }
}