using System.Net;

namespace Mjc.Templates.WebApi.Core.ValueObjects.ApiErrors
{
    public class NotFoundError : ApiError
    {
        public NotFoundError()
            : base(404, HttpStatusCode.NotFound.ToString())
        {
        }

        public NotFoundError(string message)
            : base(404, HttpStatusCode.NotFound.ToString(), message)
        {
        }

        public NotFoundError(string message, ErrorCode errorCode)
            : base(404, HttpStatusCode.NotFound.ToString(), message, errorCode)
        {
        }
    }
}
