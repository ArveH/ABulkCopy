namespace AParser;

public class TokenFactory : ITokenFactory
{
    public IToken GetToken(TokenName name, int startPos)
    {
        switch (name)
        {
            case TokenName.CommaToken:
                return new CommaToken(startPos);
            case TokenName.LeftParenthesesToken:
                return new LeftParenthesesToken(startPos);
            case TokenName.NameToken:
                return new NameToken(startPos);
            case TokenName.NumberToken:
                return new NumberToken(startPos);
            case TokenName.RightParenthesesToken:
                return new RightParenthesesToken(startPos);
            case TokenName.SquareLeftParenthesesToken:
                return new SquareLeftParenthesesToken(startPos);
            case TokenName.SquareRightParenthesesToken:
                return new SquareRightParenthesesToken(startPos);
            case TokenName.EofToken:
                return new EofToken(startPos);
            case TokenName.UndefinedToken:
                return UndefinedToken.Instance;
            default:
                throw new ArgumentOutOfRangeException(nameof(name), name, null);
        }
    }
}