namespace AParser.Exceptions;

public class UnexpectedTokenException : Exception
{
    public NodeType ExpectedNodeType { get; }
    public TokenName CurrentTokenName { get; }

    public UnexpectedTokenException(NodeType expectedNodeType, TokenName currentTokenName)
    : base(ErrorMessages.UnexpectedNode(expectedNodeType, currentTokenName))
    {
        ExpectedNodeType = expectedNodeType;
        CurrentTokenName = currentTokenName;
    }
}