using System.Net;

namespace Mjc.Templates.WebApi.Core.ValueObjects.ApiErrors
{
    internal class ForbiddenError : ApiError
    {
        public ForbiddenError()
            : base(403, HttpStatusCode.Forbidden.ToString())
        {
        }

        public ForbiddenError(string message)
            : base(403, HttpStatusCode.Forbidden.ToString(), message)
        {
        }

        public ForbiddenError(string message, ErrorCode errorCode)
            : base(403, HttpStatusCode.Forbidden.ToString(), message, errorCode)
        {
        }
    }
}
