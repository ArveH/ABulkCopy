namespace AParser.Tokens;

public class CommaToken : IToken
{
    public CommaToken(int startPos)
    {
        StartPos = startPos;
        Type = TokenType.CommaToken;
    }
    public TokenType Type { get; }
    public int StartPos { get; }
    public int Length
    {
        get => 1;
        set => throw new SetLengthException(TokenType.CommaToken);
    }
}