namespace ABulkCopy.Common.Graph.Visitors;

public interface IVisitor
{
    public int NodeCount { get; }
    public void Visit(INode node, int depth);
}