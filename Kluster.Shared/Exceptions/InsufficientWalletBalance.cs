namespace Kluster.Shared.Exceptions;

public class InsufficientWalletBalance : Exception
{
    public InsufficientWalletBalance()
    {
    }

    public InsufficientWalletBalance(string message) : base(message)
    {
    }

    public InsufficientWalletBalance(string message, Exception innerException) : base(message, innerException)
    {
    }
}