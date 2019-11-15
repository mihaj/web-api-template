using System.Net;

namespace Mjc.Templates.WebApi.Core.ValueObjects.ApiErrors
{
    public class InternalServerError : ApiError
    {
        public InternalServerError()
            : base(500, HttpStatusCode.InternalServerError.ToString())
        {
        }

        public InternalServerError(string message)
            : base(500, HttpStatusCode.InternalServerError.ToString(), message)
        {
        }

        public InternalServerError(string message, ErrorCode errorCode)
            : base(500, HttpStatusCode.InternalServerError.ToString(), message, errorCode)
        {
        }
    }
}
