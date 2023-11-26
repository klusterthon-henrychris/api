namespace Kluster.Shared.Exceptions;

public class WalletDoesNotExistException : Exception
{
    public WalletDoesNotExistException()
    {
    }

    public WalletDoesNotExistException(string message) : base(message)
    {
    }

    public WalletDoesNotExistException(string message, Exception innerException) : base(message, innerException)
    {
    }
}