using ABulkCopy.Common.Graph.Visitors;

namespace ABulkCopy.Common.Graph;

public class VisitorFactory : IVisitorFactory
{
    public IVisitor GetCounterVisitor()
    {
        return new CounterVisitor();
    }

    public IToStringVisitor GetToStringVisitor()
    {
        return new ToStringVisitor();
    }

    public IAddNodeVisitor GetAddNodeVisitor(Node node)
    {
        return new AddNodeVisitor(node);
    }
}