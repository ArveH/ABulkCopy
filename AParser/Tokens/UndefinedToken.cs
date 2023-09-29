namespace AParser.Tokens;

public class UndefinedToken : IToken
{
    public TokenName Name => TokenName.UndefinedToken;
    public string? ExpectedSpelling => null;
    public int StartPos => -1;
    public int Length { 
        get => throw new NotImplementedException("UndefinedToken doesn't have Length");
        set => throw new NotImplementedException("UndefinedToken doesn't have Length");
    }
}