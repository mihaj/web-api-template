using System.Net;

namespace Mjc.Templates.WebApi.Core.ValueObjects.ApiErrors
{
    public class ValidationError : ApiError
    {
        public ValidationError()
            : base(400, HttpStatusCode.BadRequest.ToString())
        {
        }

        public ValidationError(string message)
            : base(400, HttpStatusCode.BadRequest.ToString(), message)
        {
        }

        public ValidationError(string message, ErrorCode errorCode)
            : base(400, HttpStatusCode.BadRequest.ToString(), message, errorCode)
        {
        }
    }
}
