using System;
using System.Net;
using System.Runtime.Serialization;

namespace Rinkudesu.Gateways.Clients.Exceptions;

[Serializable]
public class UnexpectedResponseException : ResponseException
{
    public UnexpectedResponseException()
    {
    }

    public UnexpectedResponseException(string message) : base(message)
    {
    }

    public UnexpectedResponseException(string message, Exception inner) : base(message, inner)
    {
    }

    public UnexpectedResponseException(HttpStatusCode statusCode, string clientName) : this(
        $"Unexpected response received from client {clientName} - response code {statusCode}")
    {
    }

    protected UnexpectedResponseException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}
