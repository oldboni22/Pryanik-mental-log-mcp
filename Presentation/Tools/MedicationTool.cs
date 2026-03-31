using System.ComponentModel;
using System.Text.Json;
using APP.Services;
using ModelContextProtocol.Server;

namespace Presentation.Tools;

[McpServerToolType]
[Description(ToolMetadata.MedicationSafeguard)]
public static class MedicationTool
{
    [McpServerTool, Description("Adds a medication record. " + ToolMetadata.MedicationSafeguard)]
    public static async Task<string> CreateMedication(
        MedicationService service,
        [Description("Name of the item.")] string name,
        [Description("Requires prescription?")] bool prescriptionRequired,
        [Description("Description and dosage.")] string description,
        [Description("Currently taking?")] bool currentlyTaking)
    {
        await service.CreateMedication(name, prescriptionRequired, description, currentlyTaking);
        return $"{ToolMetadata.Created} Note: Follow safeguards.";
    }

    [McpServerTool, Description("Updates intake status.")]
    public static async Task<string> UpdateTakingStatus(
        MedicationService service,
        [Description(ToolMetadata.MedicationId)] Guid medicationId,
        [Description("New status.")] bool newStatus)
    {
        var success = await service.UpdateTakingStatus(medicationId, newStatus);
        return success ? ToolMetadata.Updated : ToolMetadata.NotFound;
    }

    [McpServerTool, Description("Finds medications using semantic search.")]
    public static async Task<string> GetSemanticMedication(
        MedicationService service,
        [Description(ToolMetadata.Query)] string query,
        [Description(ToolMetadata.Limit)] int outputLimit,
        [Description(ToolMetadata.MinScore)] float minScore)
    {
        var medications = await service.GetSemanticMedication(query, outputLimit, minScore);
        return JsonSerializer.Serialize(medications);
    }
}