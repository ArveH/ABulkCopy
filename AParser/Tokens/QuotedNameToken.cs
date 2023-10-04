namespace AParser.Tokens;

public class QuotedNameToken : IToken
{
    public QuotedNameToken(int startPos)
    {
        StartPos = startPos;
        Name = TokenName.QuotedNameToken;
    }
    public TokenName Name { get; }
    public int StartPos { get; }
    public int Length { get; set; }
}