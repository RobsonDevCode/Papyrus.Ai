namespace Papyrus.Domain.Exceptions;

public class NoteNotFoundException : Exception
{
    public NoteNotFoundException() : base("Note not found"){}
    
    public NoteNotFoundException(string message) : base(message){}
    
    public NoteNotFoundException(string message, Exception innerException) : base(message, innerException){}
}