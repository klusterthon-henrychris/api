namespace Kluster.Shared.Exceptions;

public class PaymentAlreadyExistsException: Exception
{
    public PaymentAlreadyExistsException()
    {
    }

    public PaymentAlreadyExistsException(string message) : base(message)
    {
    }

    public PaymentAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}