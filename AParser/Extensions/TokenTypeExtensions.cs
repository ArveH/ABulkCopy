namespace AParser.Extensions;

public static class TokenTypeExtensions
{
    public static NodeType ToNodeType(this TokenType type)
    {
        switch (type)
        {
            case TokenType.CommaToken:
                return NodeType.CommaNode;
            case TokenType.LeftParenthesesToken:
                return NodeType.LeftParenthesesNode;
            case TokenType.NameToken:
                return NodeType.NameNode;
            case TokenType.NumberToken:
                return NodeType.NumberNode;
            case TokenType.QuotedNameToken:
                return NodeType.QuotedNameNode;
            case TokenType.RightParenthesesToken:
                return NodeType.RightParenthesesNode;
            default:
                throw new ConversionException(type.ToString(), nameof(NodeType));
        }
    }
}