namespace AParser.Tree.Nodes.AbstractNodes;

public abstract class ExpressionNode : INode
{
    public abstract NodeType NodeType { get; }
    public abstract T Create<T>();
}