namespace APP.DataModels;

public sealed record ChunkModel(string Text, Guid EntryId, string EntrySummary, DateTime TimeStamp, int Number, int TotalChunks);
