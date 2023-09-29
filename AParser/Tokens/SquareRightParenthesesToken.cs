namespace AParser.Tokens;

public class SquareRightParenthesesToken : IToken
{
    public SquareRightParenthesesToken(int startPos)
    {
        StartPos = startPos;
        Name = TokenName.SquareRightParenthesesToken;
    }
    public TokenName Name { get; }
    public int StartPos { get; }
    public int Length
    {
        get => 1;
        set => throw new SetLengthException(TokenName.SquareRightParenthesesToken);
    }
}