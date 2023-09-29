namespace AParser.ParseTree.Nodes;

public class ConstantLeafNode : ILeafNode
{
    public ConstantLeafNode(IToken token)
    {
        Token = token;
    }

    public NodeType Type => NodeType.ConstantLeafNode;
    public bool IsLeafNode => true;
    public void Create(ITokenizer tokenizer)
    {
        throw new NotImplementedException();
    }

    public IToken Token { get; }
}