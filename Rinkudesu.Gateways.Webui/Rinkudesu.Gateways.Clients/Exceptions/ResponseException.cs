using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Rinkudesu.Gateways.Clients.Exceptions;

[Serializable]
[ExcludeFromCodeCoverage]
public class ResponseException : Exception
{
    public ResponseException()
    {
    }

    public ResponseException(string message) : base(message)
    {
    }

    public ResponseException(string message, Exception inner) : base(message, inner)
    {
    }

    protected ResponseException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}
