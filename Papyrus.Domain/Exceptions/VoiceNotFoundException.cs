namespace Papyrus.Domain.Exceptions;

public class VoiceNotFoundException : Exception
{
    public VoiceNotFoundException() : base("Voice not found") {}
    public VoiceNotFoundException(string message) : base(message) {}
    public VoiceNotFoundException(string message, Exception innerException) : base(message, innerException) {}
    
}