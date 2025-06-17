namespace Papyrus.Domain.Clients.Prompts;

public static class PromptGenerator
{
    public static string BasicNotePrompt(string book, string text)
    {
        return $"""
                Act as a teacher helping a student take notes and analyze this text from the text/book {book} and create detailed bullet point notes. 
                Add context where helpful. Use plain text only.
                """;
    }
    
    public static string PromptTextWithImage(string book, string text)
    {
        return $"""
                Act as a teacher helping a student take notes and analyze this text and images the text/book {book} and create detailed bullet point notes. 
                Add context where helpful. Use plain text only. 
                $$IMAGE_X$$ markers show where images appear in the text (X = image array index). {text}";
                """;
    }

    public static string ImageFocusedNote(string book, string text)
    {
        return $"""
                Act as a teacher helping a student take notes analyze these image/images from the text/book {book} and create detailed bullet point notes.
                Add context where helpful explaining key concepts seen in the image/images you can use the text to further emphasise your points if they're are related.
                Use plain text only.
                {text}
                """;
    }
}