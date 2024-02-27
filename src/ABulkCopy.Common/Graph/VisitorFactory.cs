namespace ABulkCopy.Common.Graph;

public class VisitorFactory : IVisitorFactory
{
    public IVisitor GetCounterVisitor()
    {
        return new CounterVisitor();
    }

    public IDepthVisitor GetDepthVisitor()
    {
        return new DepthVisitor();
    }

    public IAddNodeVisitor GetAddNodeVisitor(INode node)
    {
        return new AddNodeVisitor(node);
    }
}