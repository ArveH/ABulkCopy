namespace AParser.ParseTree.Nodes;

public class CommaLeafNode : ILeafNode
{
    public CommaLeafNode(IToken token)
    {
        Token = token;
    }

    public NodeType Type => NodeType.CommaLeafNode;
    public bool IsLeafNode => true;
    public void Create(ITokenizer tokenizer)
    {
        throw new NotImplementedException();
    }

    public IToken Token { get; }
}