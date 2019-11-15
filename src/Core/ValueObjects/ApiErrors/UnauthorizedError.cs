using System.Net;

namespace Mjc.Templates.WebApi.Core.ValueObjects.ApiErrors
{
    internal class UnauthorizedError : ApiError
    {
        public UnauthorizedError()
            : base(401, HttpStatusCode.Unauthorized.ToString())
        {
        }

        public UnauthorizedError(string message)
            : base(401, HttpStatusCode.Unauthorized.ToString(), message)
        {
        }

        public UnauthorizedError(string message, ErrorCode errorCode)
            : base(401, HttpStatusCode.Unauthorized.ToString(), message, errorCode)
        {
        }
    }
}
