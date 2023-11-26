using System;

namespace KLA.Desktop.Exceptions;

public class ServerException : Exception
{
    public ServerException(string? message) : base(message)
    {
    }
}