using System.ComponentModel;
using System.Text.Json;
using APP;
using ModelContextProtocol.Server;

namespace Presentation.Tools;

[McpServerToolType]
public static class AdviceTool
{
    private const string LimitDescription = "The limit of fetched advices.";
    
    [McpServerTool, Description("Creates a new advice.")]
    public static async Task<string> CreateAdvice(
        LogService service,
        [Description("The topic of the advice.")] string topic,
        [Description("The summary of the advice.")] string summary,
        [Description("The text of the advice.")] string text,
        [Description("The id of the related entry.")] Guid? sourceEntryId)
    {
        await  service.CreateAdvice(topic, summary, text, sourceEntryId);
        
        return "Advice created successfully.";
    }

    [McpServerTool, Description("Remove a specific advice.")]
    public static async Task<string> RemoveAdvice(
        LogService service,
        [Description("The id of the desired advice.")] Guid adviceId)
    {
        await service.RemoveAdvice(adviceId);
        
        return "Advice removed successfully.";
    }
    
    [McpServerTool, Description("Get recent advice summaries.")]
    public static async Task<string> GetRecentAdviceSummaries(
        LogService service,
        [Description(LimitDescription)] int outputLimit)
    {
        var summaries = await service.GetRecentAdviceSummaries(outputLimit);
        return JsonSerializer.Serialize(summaries);
    }

    [McpServerTool, Description("Get summaries of the advices related to the target entry.")]
    public static async Task<string> GetEntryAdviceSummaries(
        LogService service,
        [Description("The id of the target entry.")] Guid entryId)
    {
        var summaries = await service.GetEntryAdviceSummaries(entryId);
        return JsonSerializer.Serialize(summaries);
    }
    
    [McpServerTool, Description("Get full content of the advice.")]
    public static async Task<string> GetFullAdvice(
        LogService service,
        [Description("The id of the desired advice.")] Guid adviceId)
    {
        var advice = await service.GetFullAdvice(adviceId);
        return advice is null 
            ? "Advice not found."
            : JsonSerializer.Serialize(advice);
    }

    [McpServerTool, Description("Get summaries of advices matching the query.")]
    public static async Task<string> GetSemanticAdviceSummary(
        LogService service,
        [Description("The query.")] string query,
        [Description(LimitDescription)] int outputLimit,
        [Description("The minimum similarity score threshold.")] float minScore)
    {
        var summaries = await service.GetSemanticAdviceSummary(query, outputLimit, minScore);
        return JsonSerializer.Serialize(summaries);
    }
}
