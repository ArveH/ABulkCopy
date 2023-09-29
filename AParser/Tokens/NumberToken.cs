namespace AParser.Tokens;

public class NumberToken : IToken
{
    public NumberToken(int startPos)
    {
        StartPos = startPos;
        Name = TokenName.NumberToken;
    }
    public TokenName Name { get; }
    public int StartPos { get; }
    public int Length { get; set; }
}