namespace AParser.Tokens;

public class NStringToken : IToken
{
    public NStringToken(int startPos)
    {
        StartPos = startPos;
        Type = TokenType.NStringToken;
    }
    public TokenType Type { get; }
    public int StartPos { get; }
    public int Length { get; set; }
}