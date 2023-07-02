namespace ABulkCopy.Common.Graph;

public interface IVisitorFactory
{
    ICounterVisitor GetCounterVisitor();
    IAddNodeVisitor GetAddNodeVisitor(Node node);
}