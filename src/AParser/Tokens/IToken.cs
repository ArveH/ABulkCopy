namespace AParser.Tokens;

public interface IToken
{
    public TokenType Type { get; }
    public int StartPos { get; }
    public int Length { get; set; }
}