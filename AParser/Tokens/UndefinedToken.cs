namespace AParser.Tokens;

public class UndefinedToken : IToken
{
    private UndefinedToken()
    { }

    public TokenName Name => TokenName.UndefinedToken;
    public int StartPos => -1;
    public int Length { 
        get => throw new NotImplementedException("UndefinedToken doesn't have Length");
        set => throw new NotImplementedException("UndefinedToken doesn't have Length");
    }

    public static IToken Instance { get; } = new UndefinedToken();
}