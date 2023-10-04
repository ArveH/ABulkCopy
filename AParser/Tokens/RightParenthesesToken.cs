namespace AParser.Tokens;

public class RightParenthesesToken : IToken
{
    public RightParenthesesToken(int startPos)
    {
        StartPos = startPos;
        Type = TokenType.RightParenthesesToken;
    }
    public TokenType Type { get; }
    public int StartPos { get; }
    public int Length
    {
        get => 1;
        set => throw new SetLengthException(TokenType.RightParenthesesToken);
    }
}