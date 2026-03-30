namespace APP.DataModels.Entry;

public sealed record EntrySummaryModel(Guid EntryId, string Summary, int TextLength, DateTime Timestamp);
