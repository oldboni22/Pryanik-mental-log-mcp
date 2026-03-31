using System.ComponentModel;
using System.Text.Json;
using APP.Services;
using ModelContextProtocol.Server;

namespace Presentation.Tools;

[McpServerToolType]
public static class AdviceTool
{
    [McpServerTool, Description("Creates a new advice.")]
    public static async Task<string> CreateAdvice(
        AdviceService service,
        [Description("Advice topic.")] string topic,
        [Description("Advice summary.")] string summary,
        [Description("Full advice text.")] string text,
        [Description("Source entry ID.")] Guid? sourceEntryId)
    {
        await service.CreateAdvice(topic, summary, text, sourceEntryId);
        return ToolMetadata.Created;
    }

    [McpServerTool, Description("Removes a specific advice.")]
    public static async Task<string> RemoveAdvice(
        AdviceService service,
        [Description(ToolMetadata.AdviceId)] Guid adviceId)
    {
        await service.RemoveAdvice(adviceId);
        return ToolMetadata.Removed;
    }

    [McpServerTool, Description("Finds advice using semantic search.")]
    public static async Task<string> GetSemanticAdviceSummary(
        AdviceService service,
        [Description(ToolMetadata.Query)] string query,
        [Description(ToolMetadata.Limit)] int outputLimit,
        [Description(ToolMetadata.MinScore)] float minScore)
    {
        var summaries = await service.GetSemanticAdviceSummary(query, outputLimit, minScore);
        return JsonSerializer.Serialize(summaries);
    }
}