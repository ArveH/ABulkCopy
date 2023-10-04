namespace AParser;

public class TokenFactory : ITokenFactory
{
    public IToken GetToken(TokenType type, int startPos)
    {
        switch (type)
        {
            case TokenType.CommaToken:
                return new CommaToken(startPos);
            case TokenType.LeftParenthesesToken:
                return new LeftParenthesesToken(startPos);
            case TokenType.NameToken:
                return new NameToken(startPos);
            case TokenType.NumberToken:
                return new NumberToken(startPos);
            case TokenType.QuotedNameToken: 
                return new QuotedNameToken(startPos);
            case TokenType.RightParenthesesToken:
                return new RightParenthesesToken(startPos);
            case TokenType.EofToken:
                return new EofToken(startPos);
            case TokenType.UndefinedToken:
                return UndefinedToken.Instance;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}