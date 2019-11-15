using System.Text.Json.Serialization;

namespace Mjc.Templates.WebApi.Core.ValueObjects.ApiErrors
{
    public class ApiError
    {
        public int StatusCode { get; private set; }

        public string StatusDescription { get; private set; }

        [JsonPropertyName("message")]
        public string Message { get; private set; }

        public int ErrorCode { get; private set; }

        public ApiError(int statusCode, string statusDescription)
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }

        public ApiError(int statusCode, string statusDescription, string message)
            : this(statusCode, statusDescription)
        {
            Message = message;
        }

        public ApiError(int statusCode, string statusDescription, string message,
                        ErrorCode errorCode)
            : this(statusCode, statusDescription)
        {
            Message = message;
            ErrorCode = errorCode.ToCode();
        }
    }

    public class ErrorCode
    {
        private readonly string _name;
        private readonly int _value;

        public static readonly ErrorCode GENERAL_ERROR = new ErrorCode(1000, "General Error.");
        public static readonly ErrorCode UNAUTHORIZED_ERROR = new ErrorCode(1001, "Unauthorized.");
        public static readonly ErrorCode NOT_FOUND = new ErrorCode(1002, "Not found.");
        public static readonly ErrorCode VALIDATION_ERROR = new ErrorCode(1003, "Validation error.");
        public static readonly ErrorCode SERVER_CONNECTION_ERROR = new ErrorCode(1004, "Server connection error.");
        public static readonly ErrorCode FORBIDDEN_ERROR = new ErrorCode(1005, "Forbidden.");

        private ErrorCode(int value, string name)
        {
            _name = name;
            _value = value;
        }

        /// <summary>
        /// To String override
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _name;
        }

        /// <summary>
        /// Return code as integer
        /// </summary>
        /// <returns></returns>
        public int ToCode()
        {
            return _value;
        }
    }
}
