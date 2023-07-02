namespace ABulkCopy.Common.Graph;

public class VisitorFactory : IVisitorFactory
{
    public ICounterVisitor GetCounterVisitor()
    {
        return new CounterVisitor();
    }

    public IAddNodeVisitor GetAddNodeVisitor(Node node)
    {
        return new AddNodeVisitor(node);
    }
}