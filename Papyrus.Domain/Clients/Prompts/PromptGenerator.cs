namespace Papyrus.Domain.Clients.Prompts;

public static class PromptGenerator
{
    public static string PagePrompt(string book)
    {
        return $"""
                You are an expert teacher helping a student create comprehensive study notes. Please analyze this entire PDF page from "{book}" and generate detailed bullet point notes covering all content.

                Instructions:
                - Extract all key information, concepts, and details from the entire page
                - Analyze all text content: headings, paragraphs, captions, sidebars, and any other textual elements
                - Interpret all visual elements: diagrams, charts, figures, images, tables, and graphs
                - Explain the significance and meaning of all visual content
                - Connect and integrate information between text and visual elements
                - Identify the main topics and subtopics covered on the page
                - Add explanatory context for complex concepts to aid understanding
                - Organize information logically by topic, concept, or the page's natural flow
                - Use clear, student-friendly language
                - Focus on information that would be important for studying or review
                - Ensure comprehensive coverage of the entire page content

                Format requirements:
                - Use bullet points with sub-bullets for supporting details
                - Start with the main topic/concept as a header
                - Keep explanations concise but thorough
                - Use plain text only (no special formatting)

                IMPORTANT: Return ONLY the notes. Do not include any opening statements, introductory text, concluding remarks, or commentary. Begin immediately with the notes content.
                """;
    }
    
    public static string BasicNotePrompt(string book, string text)
    {
        return $"""
                 You are an expert teacher helping a student create comprehensive study notes. Please analyze the following text from "{book}" and generate detailed bullet point notes.
                 
                 Instructions:
                 - Extract all key information, concepts, and main points from the text
                 - Identify important terminology and definitions
                 - Summarize supporting details and examples
                 - Add explanatory context for complex concepts to aid understanding
                 - Organize information logically by topic or concept
                 - Use clear, student-friendly language
                 - Focus on information that would be important for studying or review
                 
                 Format requirements:
                 - Use bullet points with sub-bullets for supporting details
                 - Start with the main topic/concept as a header
                 - Keep explanations concise but thorough
                 - Use plain text only (no special formatting)
                 
                 IMPORTANT: Return ONLY the notes. Do not include any opening statements, introductory text, concluding remarks, or commentary. Begin immediately with the notes content.
                 
                 {text}
                 """;
    }
    
    public static string PromptTextWithImage(string book, string text)
    {
        return $"""
                 You are an expert teacher helping a student create comprehensive study notes. Please analyze the text and images from this PDF page of "{book}" and generate detailed bullet point notes.
                 Focus on the text attached as this is what the student has asked to focus the notes on. 
                 $$IMAGE_X$$ markers show where the images on the pdf page appear in the text (X = image array index).
                 
                 Instructions:
                 - Extract all key information, concepts, and details from both text and images
                 - For text content: identify main points, supporting details, and key terminology
                 - For images, diagrams, charts, or figures: explain what they show and their significance
                 - Connect related information between text and visual elements when applicable
                 - Add explanatory context for complex concepts to aid understanding
                 - Organize information logically by topic or concept
                 - Use clear, student-friendly language
                 - Focus on information that would be important for studying or review
                 
                 
                 Format requirements:
                 - Use bullet points with sub-bullets for supporting details
                 - Start with the main topic/concept as a header
                 - Keep explanations concise but thorough
                 - Use plain text only (no special formatting)
                 
                 IMPORTANT: Return ONLY the notes. Do not include any opening statements, introductory text, concluding remarks, or commentary. Begin immediately with the notes content.
                 
                 {text}
                 """;
    }

    public static string ImageFocusedNote(string book, int imageReference)
    {
        return $"""
                 You are an expert teacher helping a student create comprehensive study notes. Please analyze the image from the PDF page of "{book}" and generate detailed bullet point notes.
                 $$IMAGE_X$$ markers show where the images on the pdf page appear in the text (X = image array index).
                 Analyze Page $$IMAGE_{imageReference}$$
                 
                 Instructions:
                 - Extract all key information, concepts, and details visible in the image
                 - For diagrams, charts, or figures: explain what they show and their significance
                 - Add explanatory context for complex concepts to aid understanding
                 - If multiple related concepts appear, organize them logically
                 - Use clear, student-friendly language
                 - Focus on information that would be important for studying or review
                 
                 Format requirements:
                 - Use bullet points with sub-bullets for supporting details
                 - Start with the main topic/concept as a header
                 - Keep explanations concise but thorough
                 - Use plain text only (no special formatting)
                 
                 IMPORTANT: Return ONLY the notes. Do not include any opening statements, introductory text, concluding remarks, or commentary. Begin immediately with the notes content.
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