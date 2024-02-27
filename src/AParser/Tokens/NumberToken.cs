namespace AParser.Tokens;

public class NumberToken : IToken
{
    public NumberToken(int startPos)
    {
        StartPos = startPos;
        Type = TokenType.NumberToken;
    }
    public TokenType Type { get; }
    public int StartPos { get; }
    public int Length { get; set; }
}