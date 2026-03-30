namespace APP.DataModels.Advice;

public sealed record AdviceSummaryModel(Guid AdviceId, string Topic, string Summary, int TextLength, DateTime TimeStamp, Guid? EntryId);
