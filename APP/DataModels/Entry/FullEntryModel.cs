namespace APP.DataModels.Entry;

public sealed record FullEntryModel(Guid EntryId, string Summary, string FullText, DateTime Timestamp);
