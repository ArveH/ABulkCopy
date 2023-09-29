namespace AParser.ParseTree;

public interface ILeafNode : INodeBase
{
    IToken Token { get; }
}