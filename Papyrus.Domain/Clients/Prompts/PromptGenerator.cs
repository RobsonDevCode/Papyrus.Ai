namespace Papyrus.Domain.Clients.Prompts;

public static class PromptGenerator
{
    public static string BasicNotePrompt(string book, string text)
    {
        return $"""
                Act as a teacher helping a student take notes and analyze this text from the text/book {book} and create detailed bullet point notes. 
                Add context where helpful. Use plain text only.
                {text}
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

    public static string ImproveNotePrompt(string previousNote, string userPrompt, string book, string context)
    {
        return $"""
                 You are a teacher improving student notes. Your task is to ONLY output the improved note - no explanations or commentary.
                 
                 STUDENT REQUEST: {userPrompt}
                 
                 PREVIOUS NOTE TO IMPROVE: {previousNote}
                 
                 REFERENCE MATERIALS:
                 - Original text: {context}
                 - Source: {book}
                 - Images appear at $$IMAGE_X$$ markers (X = image index)
                 
                 INSTRUCTIONS:
                 1. Improve the note based on the student's request
                 2. Add helpful context and key concept explanations
                 3. Reference images when relevant using $$IMAGE_X$$ markers
                 4. Use plain text only
                 5. Output ONLY the improved note - no other text
                 
                 IMPROVED NOTE:
                 """;
    }
}