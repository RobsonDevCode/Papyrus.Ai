namespace Papyrus.Domain.Clients.Prompts;

public static class PromptGenerator
{
    public static string BasicNotePrompt(string book, string text)
    {
        return $"""
                Create detailed notes on this text using numbered or normal bullet points from {book}. 
                Add relevant context and supplementary information where helpful. 
                Provide only the notes without commentary. {text}";
                """;
    }
    
    public static string PromptTextWithImage(string book, string text)
    {
        return $"""
                Analyze this text and images from {book} and create detailed bullet point notes. 
                Add context where helpful. Use plain text only. 
                $$IMAGE_X$$ markers show where images appear in the text (X = image array index). {text}";
                """;
    }
    
}