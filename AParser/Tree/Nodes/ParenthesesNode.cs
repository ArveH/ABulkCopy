namespace AParser.Tree.Nodes;

public class ParenthesesNode : ExpressionNode
{
    public override NodeType NodeType => NodeType.ParenthesesNode;
    public required ExpressionNode Expression { get; init; }

    public override T Create<T>()
    {
        throw new NotImplementedException();
    }
}