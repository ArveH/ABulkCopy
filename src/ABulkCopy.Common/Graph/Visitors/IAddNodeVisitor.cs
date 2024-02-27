namespace ABulkCopy.Common.Graph.Visitors;

public interface IAddNodeVisitor : IVisitor
{
    public bool IsAdded { get; }
}