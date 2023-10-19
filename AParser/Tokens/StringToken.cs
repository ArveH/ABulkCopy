namespace AParser.Tokens;

public class StringToken : IToken
{
    public StringToken(int startPos)
    {
        StartPos = startPos;
        Type = TokenType.StringToken;
    }
    public TokenType Type { get; }
    public int StartPos { get; }
    public int Length { get; set; }
}