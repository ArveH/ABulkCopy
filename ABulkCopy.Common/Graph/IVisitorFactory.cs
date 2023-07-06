using ABulkCopy.Common.Graph.Visitors;

namespace ABulkCopy.Common.Graph;

public interface IVisitorFactory
{
    IVisitor GetCounterVisitor();
    IDepthVisitor GetDepthVisitor();
    IAddNodeVisitor GetAddNodeVisitor(Node node);
}