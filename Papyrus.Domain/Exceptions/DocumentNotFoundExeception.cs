namespace Papyrus.Domain.Exceptions;

public class DocumentNotFoundExeception : Exception
{
    public DocumentNotFoundExeception() : base("Document not found") {}
    
    public DocumentNotFoundExeception(string message) : base(message) {}
    
    public DocumentNotFoundExeception(string message, Exception innerException) : base(message, innerException) {}
}