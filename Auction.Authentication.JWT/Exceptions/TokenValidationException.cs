using System;
using System.Runtime.Serialization;

namespace Auction.Authentication.JWT.Exceptions
{
    [Serializable]
    public class TokenValidationException : Exception
    {
        public TokenValidationException() { }

        public TokenValidationException(string message) : base(message) { }

        public TokenValidationException(string message, Exception innerException) : base(message, innerException) { }

        protected TokenValidationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
