namespace Papyrus.Domain.Clients.Prompts;

public static class PromptGenerator
{
    public static string PagePrompt(string book)
    {
        return $"""
                Please analyze this entire PDF page from "{book}" and generate detailed bullet point notes covering all content.
                """;
    }

    public static string ExplanationPrompt(bool hasImage, string textToExplain, string pageContext)
    {
        if (hasImage)
        {
            return $"""
                    I'm studying the attached page and need help understanding a specific concept or passage.

                    Please provide a clear, detailed explanation that:
                    - IMPORTANT make sure the response is no longer than 500 characters.
                    - Breaks down any complex terminology or concepts into simpler terms
                    - Uses analogies or examples where helpful to illustrate the ideas
                    - Connects the concept to other relevant information visible on this page
                    - Explains why this concept is important and how it fits into the broader topic
                    - Identifies any prerequisite knowledge I should understand first

                    Please tailor your explanation to someone actively learning this material, aiming for clarity over brevity.
                    
                    [The specific text or concept I need explained: {textToExplain}]
                    """;
        }
        
        return $"""
               I'm studying the attached page and need help understanding a specific concept or passage.
               
               Please provide a clear, detailed explanation that:
               - IMPORTANT make sure the response is no longer than 500 characters.
               - Breaks down any complex terminology or concepts into simpler terms
               - Uses analogies or examples where helpful to illustrate the ideas
               - Connects the concept to other relevant information visible on this page
               - Explains why this concept is important and how it fits into the broader topic
               - Identifies any prerequisite knowledge I should understand first
               
               Please tailor your explanation to someone actively learning this material, aiming for clarity over brevity.
               
               [The specific text or concept I need explained: {textToExplain}]
               
               [The entire page for context: {pageContext}] 
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