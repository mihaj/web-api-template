using System.Net;

namespace Mjc.Templates.WebApi.Core.ValueObjects.ApiErrors
{
    public class BadRequestError : ApiError
    {
        public BadRequestError()
            : base(400, HttpStatusCode.BadRequest.ToString())
        {
        }

        public BadRequestError(string message)
            : base(400, HttpStatusCode.BadRequest.ToString(), message)
        {
        }

        public BadRequestError(string message, ErrorCode errorCode)
          : base(400, HttpStatusCode.BadRequest.ToString(), message, errorCode)
        {
        }
    }
}
