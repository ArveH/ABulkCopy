namespace ABulkCopy.Common.Graph.Visitors;

public class DepthVisitor : VisitorBase, IDepthVisitor
{
    private readonly List<TableDepth> _result = new();

    public List<TableDepth> Result => _result;

    public override void Visit(Node node, int depth)
    {
        if (!node.IsRoot)
        {
            base.Visit(node, depth);
            _result.Add(new TableDepth(node.Name, depth));
        }
    }
}