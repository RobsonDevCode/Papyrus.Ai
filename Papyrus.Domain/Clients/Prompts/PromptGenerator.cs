namespace Papyrus.Domain.Clients.Prompts;

public static class PromptGenerator
{
    public static string PagePrompt(string book)
    {
        return $"""
                Please analyze this entire PDF page from "{book}" and generate detailed bullet point notes covering all content.
                """;
    }
    
    public static string BasicNotePrompt(string book, string text)
    {
        return $"""
                 Please analyze the following text from "{book}" and generate detailed bullet point notes.
                 {text}
                 """;
    }
    
    public static string PromptTextWithImage(string book, string text)
    {
        return $"""
                 Please analyze the text and images from this PDF page of "{book}" and generate detailed bullet point notes.
                 Focus on the text attached as this is what the student has asked to focus the notes on. 
                 $$IMAGE_X$$ markers show where the images on the pdf page appear in the text (X = image array index).
                 {text}
                 """;
    }

    public static string ImageFocusedNote(string book, int imageReference)
    {
        return $"""
                 Please analyze the image from the PDF page of "{book}" and generate detailed bullet point notes.
                 $$IMAGE_X$$ markers show where the images on the pdf page appear in the text (X = image array index).
                 Analyze Page $$IMAGE_{imageReference}$$
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