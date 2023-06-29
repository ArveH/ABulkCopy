namespace ABulkCopy.Common.Graph;

public class CounterVisitor : ICounterVisitor
{
    public int Count { get; private set; }

    public CounterVisitor()
    {
        Count = 0;
    }

    public void Visit(Node _)
    {
        Count++;
    }
}