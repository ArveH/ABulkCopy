namespace ABulkCopy.Common.Graph;

public class VisitorFactory : IVisitorFactory
{
    public ICounterVisitor GetCounterVisitor()
    {
        return new CounterVisitor();
    }
}