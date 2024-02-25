namespace AParser.Tokens;

public class NameToken : IToken
{
    public NameToken(int startPos)
    {
        StartPos = startPos;
        Type = TokenType.NameToken;
    }
    public TokenType Type { get; }
    public int StartPos { get; }
    public int Length { get; set; }
}