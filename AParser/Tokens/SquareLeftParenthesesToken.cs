namespace AParser.Tokens;

public class SquareLeftParenthesesToken : IToken
{
    public SquareLeftParenthesesToken(int startPos)
    {
        StartPos = startPos;
        Name = TokenName.SquareLeftParenthesesToken;
    }
    public TokenName Name { get; }
    public string? ExpectedSpelling => "[";
    public int StartPos { get; }
    public int Length
    {
        get => 1;
        set => throw new SetLengthException(TokenName.SquareLeftParenthesesToken);
    }
}