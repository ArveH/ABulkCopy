namespace AParser.KnownTokens;

public class LeftParenthesesToken : IToken
{
    public LeftParenthesesToken(int startPos)
    {
        StartPos = startPos;
        Name = TokenName.LeftParenthesesToken;
    }
    public TokenName Name { get; }
    public string? ExpectedSpelling => "(";
    public int StartPos { get; }
    public int Length
    {
        get => 1;
        set => throw new SetLengthException(TokenName.LeftParenthesesToken);
    }
}