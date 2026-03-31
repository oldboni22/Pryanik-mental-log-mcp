using System.ComponentModel;
using System.Text.Json;
using APP.Services;
using ModelContextProtocol.Server;

namespace Presentation.Tools;

[McpServerToolType]
public static class TraitTool
{
    [McpServerTool, Description("Creates a new trait with semantic embedding.")]
    public static async Task<string> CreateTrait(
        TraitService service,
        [Description("Description of the trait.")] string description,
        [Description("Optional related entry IDs.")] Guid[]? relatedEntryIds)
    {
        await service.CreateTrait(description, relatedEntryIds);
        return ToolMetadata.Created;
    }

    [McpServerTool, Description("Removes a specific trait.")]
    public static async Task<string> RemoveTrait(
        TraitService service,
        [Description(ToolMetadata.TraitId)] Guid traitId)
    {
        await service.RemoveTrait(traitId);
        return ToolMetadata.Removed;
    }

    [McpServerTool, Description("Links a trait to an entry.")]
    public static async Task<string> RelateTraitToEntry(
        TraitService service,
        [Description(ToolMetadata.TraitId)] Guid traitId,
        [Description(ToolMetadata.EntryId)] Guid entryId)
    {
        await service.RelateTraitToEntry(traitId, entryId);
        return ToolMetadata.Updated;
    }

    [McpServerTool, Description("Finds traits using semantic search.")]
    public static async Task<string> GetSemanticTraits(
        TraitService service,
        [Description(ToolMetadata.Query)] string query,
        [Description(ToolMetadata.Limit)] int outputLimit,
        [Description(ToolMetadata.MinScore)] float minScore)
    {
        var traits = await service.GetSemanticTraits(query, outputLimit, minScore);
        return JsonSerializer.Serialize(traits);
    }
}