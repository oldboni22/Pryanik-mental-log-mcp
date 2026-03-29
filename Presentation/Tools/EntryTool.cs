using System.ComponentModel;
using System.Text.Json;
using APP;
using APP.Services;
using ModelContextProtocol.Server;

namespace Presentation.Tools;

[McpServerToolType]
public static class EntryTool
{
    private const string LimitDescription = "The limit of fetched items.";

    [McpServerTool, Description("Creates a new log entry divided into chunks.")]
    public static async Task<string> CreateEntry(
        EntryService service,
        [Description("The summary of the entry.")] string summary,
        [Description("The text of the entry, split into an array of chunks. CRITICAL: Split the text into logical, self-contained units to optimize for RAG semantic search. Each chunk MUST NOT exceed 10 sentences. Do not break thoughts in the middle.")] 
        string[] chunks)
    {
        await service.CreateEntry(summary, chunks);
        
        return "Entry created successfully.";
    }

    [McpServerTool, Description("Removes a specific log entry.")]
    public static async Task<string> RemoveEntry(
        EntryService service,
        [Description("The id of the desired entry.")] Guid entryId)
    {
        await service.RemoveEntry(entryId);
        
        return "Entry removed successfully.";
    }

    [McpServerTool, Description("Get recent entry summaries.")]
    public static async Task<string> GetRecentEntrySummaries(
        EntryService service,
        [Description(LimitDescription)] int outputLimit)
    {
        var summaries = await service.GetRecentEntrySummaries(outputLimit);
        return JsonSerializer.Serialize(summaries);
    }

    [McpServerTool, Description("Get full content of the log entry.")]
    public static async Task<string> GetFullEntry(
        EntryService service,
        [Description("The id of the desired entry.")] Guid entryId)
    {
        var entry = await service.GetFullEntry(entryId);
        return entry is null 
            ? "Entry not found." 
            : JsonSerializer.Serialize(entry);
    }

    [McpServerTool, Description("Get semantic entry chunks matching the query.")]
    public static async Task<string> GetSemanticChunks(
        EntryService service,
        [Description("The query for semantic search.")] string query,
        [Description(LimitDescription)] int outputLimit,
        [Description("The minimum similarity score threshold.")] float minScore)
    {
        var chunks = await service.GetSemanticChunks(query, outputLimit, minScore);
        return JsonSerializer.Serialize(chunks);
    }

    [McpServerTool, Description("Get a specific chunk of an entry by its number.")]
    public static async Task<string> GetChunk(
        EntryService service,
        [Description("The id of the target entry.")] Guid entryId,
        [Description("The ordinal number of the chunk.")] int number)
    {
        var chunk = await service.GetChunk(entryId, number);
        return chunk is null 
            ? "Chunk not found." 
            : JsonSerializer.Serialize(chunk);
    }
}