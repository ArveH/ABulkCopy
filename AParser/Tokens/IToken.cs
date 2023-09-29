namespace AParser.Tokens;

public interface IToken
{
    public TokenName Name { get; }
    public int StartPos { get; }
    public int Length { get; set; }
}