namespace ABulkCopy.Common.Graph.Visitors;

public abstract class VisitorBase : IVisitor
{
    public int NodeCount { get; protected set; }

    protected VisitorBase()
    {
        NodeCount = 0;
    }

    public virtual void Visit(Node node, int depth)
    {
        if (!node.IsRoot)
        {
            NodeCount++;
        }
    }
}