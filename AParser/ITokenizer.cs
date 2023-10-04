namespace AParser;

public interface ITokenizer
{
    string Original { get; }
    IToken CurrentToken { get; }
    string CurrentTokenText { get; }
    void Initialize(string input);
    IToken GetNext();
    IToken GetExpected(TokenName expectedToken);
    ReadOnlySpan<char> GetSpan(IToken token);
}