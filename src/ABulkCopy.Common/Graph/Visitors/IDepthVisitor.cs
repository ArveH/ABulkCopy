namespace ABulkCopy.Common.Graph.Visitors;

public interface IDepthVisitor : IVisitor
{
    public List<TableDepth> Result { get; }
}