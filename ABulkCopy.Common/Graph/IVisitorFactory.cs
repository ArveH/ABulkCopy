using ABulkCopy.Common.Graph.Visitors;

namespace ABulkCopy.Common.Graph;

public interface IVisitorFactory
{
    IVisitor GetCounterVisitor();
    IToStringVisitor GetToStringVisitor();
    IAddNodeVisitor GetAddNodeVisitor(Node node);
}