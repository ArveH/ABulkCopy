namespace ABulkCopy.Common.Graph.Visitors;

public abstract class VisitorBase : IVisitor
{
    public int Indent { get; set; } = 0;
    public int NodeCount { get; protected set; }

    protected VisitorBase()
    {
        NodeCount = 0;
    }

    public virtual void Visit(Node node)
    {
        NodeCount++;
    }
}