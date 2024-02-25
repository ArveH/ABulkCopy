namespace AParser.Tokens;

public class QuotedNameToken : IToken
{
    public QuotedNameToken(int startPos)
    {
        StartPos = startPos;
        Type = TokenType.QuotedNameToken;
    }
    public TokenType Type { get; }
    public int StartPos { get; }
    public int Length { get; set; }
}