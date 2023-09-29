namespace AParser.Tokens;

public class NameToken : IToken
{
    public NameToken(int startPos)
    {
        StartPos = startPos;
        Name = TokenName.NameToken;
    }
    public TokenName Name { get; }
    public int StartPos { get; }
    public int Length { get; set; }
}