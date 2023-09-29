namespace AParser.Tokens;

public class RightParenthesesToken : IToken
{
    public RightParenthesesToken(int startPos)
    {
        StartPos = startPos;
        Name = TokenName.RightParenthesesToken;
    }
    public TokenName Name { get; }
    public string? ExpectedSpelling => ")";
    public int StartPos { get; }
    public int Length
    {
        get => 1;
        set => throw new SetLengthException(TokenName.RightParenthesesToken);
    }
}