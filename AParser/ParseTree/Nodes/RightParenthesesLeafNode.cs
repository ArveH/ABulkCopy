namespace AParser.ParseTree.Nodes;

public class RightParenthesesLeafNode : ILeafNode
{
    public RightParenthesesLeafNode(IToken token)
    {
        Token = token;
    }

    public NodeType Type => NodeType.RightParenthesesLeafNode;
    public bool IsLeafNode => true;
    public void Create(ITokenizer tokenizer)
    {
        throw new NotImplementedException();
    }

    public IToken Token { get; }
}