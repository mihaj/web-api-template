using System;

namespace Mjc.Templates.WebApi.Core.Exceptions
{
    public class ThirdPartyException : Exception
    {
        public ThirdPartyException(string message)
            : base(message)
        { }
    }
}
