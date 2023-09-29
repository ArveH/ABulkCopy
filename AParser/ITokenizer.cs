namespace AParser;

public interface ITokenizer
{
    string Original { get; }
    void Initialize(string input);
    IToken GetNext();
}