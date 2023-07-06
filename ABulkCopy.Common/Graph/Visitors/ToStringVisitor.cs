namespace ABulkCopy.Common.Graph.Visitors;

public class ToStringVisitor : VisitorBase, IToStringVisitor
{
    private readonly StringBuilder _sb = new();

    public string Result => _sb.ToString();

    public override void Visit(Node node)
    {
        base.Visit(node);
        _sb.AppendLine($"{new string(' ', Indent)}{node.Name}");
    }
}