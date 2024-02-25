namespace ABulkCopy.Common.Graph;

public interface IVisitorFactory
{
    IVisitor GetCounterVisitor();
    IDepthVisitor GetDepthVisitor();
    IAddNodeVisitor GetAddNodeVisitor(INode node);
}