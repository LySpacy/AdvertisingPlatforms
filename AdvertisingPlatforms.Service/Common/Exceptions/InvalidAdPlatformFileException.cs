using System.Runtime.CompilerServices;

namespace AdvertisingPlatforms.Service.Common.Exceptions;

public class InvalidAdPlatformFileException : Exception
{
    public InvalidAdPlatformFileException(string message)
        : base(message)
    {}

    public InvalidAdPlatformFileException(string message, Exception? innerException)
        : base(message, innerException)
    {}
}