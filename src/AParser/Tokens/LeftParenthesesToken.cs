namespace AParser.Tokens;

public class LeftParenthesesToken : IToken
{
    public LeftParenthesesToken(int startPos)
    {
        StartPos = startPos;
        Type = TokenType.LeftParenthesesToken;
    }
    public TokenType Type { get; }
    public int StartPos { get; }
    public int Length
    {
        get => 1;
        set => throw new SetLengthException(TokenType.LeftParenthesesToken);
    }
}