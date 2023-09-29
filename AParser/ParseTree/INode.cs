namespace AParser.ParseTree;

public interface INode : INodeBase
{
    List<INode> Children { get; }
}