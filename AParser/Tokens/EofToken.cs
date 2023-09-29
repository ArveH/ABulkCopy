namespace AParser.Tokens;

public class EofToken : IToken
{
    public EofToken(int startPos)
    {
        StartPos = startPos;
    }
    public TokenName Name => TokenName.EofToken;
    public string? ExpectedSpelling => null;
    public int StartPos { get; }
    public int Length
    {
        get => 0;
        set => throw new SetLengthException(TokenName.EofToken);
    }
}