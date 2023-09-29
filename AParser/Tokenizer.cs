namespace AParser;

public class Tokenizer : ITokenizer
{
    private readonly ITokenFactory _tokenFactory;
    private string? _original;
    private int _position;

    private char CurrentChar => _position >= Original.Length ? '\0' : Original[_position];
    private char NextChar => _position + 1 >= Original.Length ? '\0' : Original[_position + 1];

    public Tokenizer(ITokenFactory tokenFactory)
    {
        _tokenFactory = tokenFactory;
    }

    public ReadOnlySpan<char> GetSpelling(IToken token)
    {
        return Original.AsSpan(token.StartPos, token.Length);
    }

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
        SkipWhitespace();
        if (_position >= Original.Length)
        {
            return new EofToken(_position);
        }

        switch (CurrentChar)
        {
            case '(':
                return _tokenFactory.GetToken(TokenName.LeftParenthesesToken, _position++);
            case ')':
                return _tokenFactory.GetToken(TokenName.RightParenthesesToken, _position++);
            case '[':
                return _tokenFactory.GetToken(TokenName.SquareLeftParenthesesToken, _position++);
            case ']':
                return _tokenFactory.GetToken(TokenName.SquareRightParenthesesToken, _position++);
            case ',':
                return _tokenFactory.GetToken(TokenName.CommaToken, _position++);
        }

        if (IsNameStartingChar(CurrentChar))
        {
            var token = _tokenFactory.GetToken(TokenName.NameToken, _position);
            SkipToEndOfName();
            token.Length = _position - token.StartPos;
            return token;
        }

        if (char.IsDigit(Original[_position]))
        {
            var token = _tokenFactory.GetToken(TokenName.NumberToken, _position);
            SkipToEndOfNumber();
            token.Length = _position - token.StartPos;
            return token;
        }

        return _tokenFactory.GetToken(TokenName.UndefinedToken, _position++);
    }

    private void SkipWhitespace()
    {
        while (char.IsWhiteSpace(CurrentChar))
        {
            _position++;
        }
    }

    private void SkipToEndOfName()
    {
        while (char.IsLetterOrDigit(CurrentChar))
        {
            _position++;
        }
    }

    private void SkipToEndOfNumber()
    {
        while (char.IsLetterOrDigit(CurrentChar))
        {
            _position++;
        }
    }

    private bool IsNameStartingChar(char c)
    {
        return char.IsLetter(c) || c == '_';
    }

    private bool IsNameChar(char c)
    {
        return char.IsLetterOrDigit(c) || c == '_';
    }
}