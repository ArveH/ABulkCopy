namespace ABulkCopy.Common.Graph.Visitors;

public interface IVisitor
{
    public int Indent { get; set; }
    public int NodeCount { get; }
    public void Visit(Node node);
}