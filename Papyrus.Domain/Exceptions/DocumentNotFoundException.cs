namespace Papyrus.Domain.Exceptions;

public class DocumentNotFoundException : Exception
{
    public DocumentNotFoundException() : base("Document not found") {}
    
    public DocumentNotFoundException(string message) : base(message) {}
    
    public DocumentNotFoundException(string message, Exception innerException) : base(message, innerException) {}
}