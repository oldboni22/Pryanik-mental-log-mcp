namespace APP.DataModels;

public sealed record FullAdviceModel(Guid AdviceId, string FullText, string Topic, string Summary, DateTime TimeStamp, Guid? EntryId);
