using ABulkCopy.Common.Graph.Visitors;

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

    public IAddNodeVisitor GetAddNodeVisitor(Node node)
    {
        return new AddNodeVisitor(node);
    }
}