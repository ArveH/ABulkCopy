namespace AParser.ParseTree.Nodes;

public class NameLeafNode : ILeafNode
{
    public NameLeafNode(IToken token)
    {
        Token = token;
    }

    public NodeType Type => NodeType.NameLeafNode;
    public bool IsLeafNode => true;
    public void Create(ITokenizer tokenizer)
    {
        throw new NotImplementedException();
    }

    public IToken Token { get; }
}