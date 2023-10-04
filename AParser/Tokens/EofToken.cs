namespace AParser.Tokens;

public class EofToken : IToken
{
    public EofToken(int startPos)
    {
        StartPos = startPos;
    }
    public TokenType Type => TokenType.EofToken;
    public int StartPos { get; }
    public int Length
    {
        get => 0;
        set => throw new SetLengthException(TokenType.EofToken);
    }
}