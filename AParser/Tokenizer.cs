namespace AParser;

public class Tokenizer : ITokenizer
{
    private string? _original;
    private int _position;

    public string Original
    {
        get => _original ?? throw new ArgumentNullException(nameof(Original), "Can't use Original string before Initialize is called");
        
        private set => _original = value;
    }

    public void Initialize(string input)
    {
        _original = input;
        _position = 0;
    }

    public IToken GetNext()
    {
        throw new NotImplementedException();
    }
}