namespace AParser.ParseTree.Nodes;

public class ConvertNode : INode
{
    public NodeType Type => NodeType.ConvertFunctionNode;
    public bool IsLeafNode => false;
    public void Create(ITokenizer tokenizer)
    {
        throw new NotImplementedException();
    }

    public List<INode> Children { get; } = new();
}