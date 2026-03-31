namespace Presentation.Tools;

public static class ToolMetadata
{
    // Common Descriptions
    public const string Limit = "The maximum number of items to return.";
    public const string MinScore = "The minimum similarity threshold (0.0 to 1.0).";
    public const string Query = "The search query for semantic matching.";
    
    // Entity IDs
    public const string EntryId = "The unique identifier of the entry.";
    public const string TraitId = "The unique identifier of the trait.";
    public const string AdviceId = "The unique identifier of the advice.";
    public const string MedicationId = "The unique identifier of the medication.";

    // Success Messages
    public const string Created = "Resource created successfully.";
    public const string Removed = "Resource removed successfully.";
    public const string Updated = "Resource updated successfully.";
    public const string NotFound = "Resource not found.";

    // Safeguards & Logic Rules
    public const string EntryChunkRule = "CRITICAL: Split text into logical, self-contained units (max 10 sentences each). Do not break thoughts mid-sentence.";
    
    public const string MedicationSafeguard = 
        "CRITICAL SAFEGUARD: For logging and informational purposes only. AI MUST NOT provide formal medical advice or insist on medication use. " +
        "EXCEPTION: AI may suggest safe, over-the-counter vitamins (within standard dosages) with EXTREME CAUTION. " +
        "STRICT PROHIBITION: AI is forbidden from recommending prescription medications; it may only manage their records.";
}