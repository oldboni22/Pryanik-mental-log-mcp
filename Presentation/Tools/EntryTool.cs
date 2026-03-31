using System.ComponentModel;
using System.Text.Json;
using APP.Services;
using ModelContextProtocol.Server;

namespace Presentation.Tools;

[McpServerToolType]
public static class EntryTool
{
    [McpServerTool, Description($"Creates a new log entry. {ToolMetadata.EntryChunkRule}")]
    public static async Task<string> CreateEntry(
        EntryService service,
        [Description("Summary of the entry.")] string summary,
        [Description("Array of semantic chunks.")] string[] chunks,
        [Description("Optional array of related trait IDs.")] Guid[]? traitIds)
    {
        await service.CreateEntry(summary, chunks, traitIds);
        return ToolMetadata.Created;
    }

    [McpServerTool, Description("Removes a specific log entry.")]
    public static async Task<string> RemoveEntry(
        EntryService service,
        [Description(ToolMetadata.EntryId)] Guid entryId)
    {
        await service.RemoveEntry(entryId);
        return ToolMetadata.Removed;
    }

    [McpServerTool, Description("Retrieves recent entry summaries.")]
    public static async Task<string> GetRecentEntrySummaries(
        EntryService service,
        [Description(ToolMetadata.Limit)] int outputLimit)
    {
        var summaries = await service.GetRecentEntrySummaries(outputLimit);
        return JsonSerializer.Serialize(summaries);
    }

    [McpServerTool, Description("Retrieves the full content of an entry.")]
    public static async Task<string> GetFullEntry(
        EntryService service,
        [Description(ToolMetadata.EntryId)] Guid entryId)
    {
        var entry = await service.GetFullEntry(entryId);
        return entry is null ? ToolMetadata.NotFound : JsonSerializer.Serialize(entry);
    }

    [McpServerTool, Description("Finds entry chunks using semantic search.")]
    public static async Task<string> GetSemanticChunks(
        EntryService service,
        [Description(ToolMetadata.Query)] string query,
        [Description(ToolMetadata.Limit)] int outputLimit,
        [Description(ToolMetadata.MinScore)] float minScore)
    {
        var chunks = await service.GetSemanticChunks(query, outputLimit, minScore);
        return JsonSerializer.Serialize(chunks);
    }
}