namespace ABulkCopy.Common.Graph;

public interface ICounterVisitor : IVisitor
{
    public int Count { get; }
}