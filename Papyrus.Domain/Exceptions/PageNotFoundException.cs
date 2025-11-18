namespace Papyrus.Domain.Exceptions;

public class PageNotFoundException : Exception
{
    public PageNotFoundException() : base("Page not found")
    {
    }

    public PageNotFoundException(string message) : base(message)
    {
    }

    public PageNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}