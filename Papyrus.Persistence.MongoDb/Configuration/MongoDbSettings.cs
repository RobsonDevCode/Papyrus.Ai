namespace Papyrus.Ai.Configuration;

public class MongoDbSettings
{
    public required string ConnectionString { get; init; }

    public required string BooksDatabase { get; init; }
    
    public required string PromptsDatabase { get; init; }
}