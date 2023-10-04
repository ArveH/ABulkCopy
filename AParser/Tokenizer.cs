namespace AParser;

public class Tokenizer : ITokenizer
{
    private readonly ITokenFactory _tokenFactory;
    private string? _original;
    private int _position;

    private char CurrentChar => _position >= Original.Length ? '\0' : Original[_position];

    public Tokenizer(ITokenFactory tokenFactory)
    {
        _tokenFactory = tokenFactory;
        CurrentToken = UndefinedToken.Instance;
    }

    public ReadOnlySpan<char> GetSpan(IToken token)
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
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new AParserException(ErrorMessages.EmptySql);
        }

        _original = input;
        _position = 0;
    }

    public IToken CurrentToken
    {
        get;
        private set;
    }

    public string CurrentTokenText => GetSpan(CurrentToken).ToString();

    public IToken GetExpected(TokenName expectedToken)
    {
        var token = GetNext();
        if (token.Name != expectedToken)
        {
            throw new UnexpectedTokenException(expectedToken, token.Name);
        }

        return token;
    }

    public IToken GetNext()
    {
        SkipWhitespace();
        if (_position >= Original.Length)
        {
            CurrentToken = new EofToken(_position);
            return CurrentToken;
        }

        switch (CurrentChar)
        {
            case '(':
                CurrentToken = _tokenFactory.GetToken(TokenName.LeftParenthesesToken, _position++);
                return CurrentToken;
            case ')':
                CurrentToken = _tokenFactory.GetToken(TokenName.RightParenthesesToken, _position++);
                return CurrentToken;
            case '[':
                CurrentToken = _tokenFactory.GetToken(TokenName.SquareLeftParenthesesToken, _position++);
                return CurrentToken;
            case ']':
                CurrentToken = _tokenFactory.GetToken(TokenName.SquareRightParenthesesToken, _position++);
                return CurrentToken;
            case ',':
                CurrentToken = _tokenFactory.GetToken(TokenName.CommaToken, _position++);
                return CurrentToken;
        }

        if (IsNameStartingChar(CurrentChar))
        {
            CurrentToken = _tokenFactory.GetToken(TokenName.NameToken, _position);
            SkipToEndOfName();
            CurrentToken.Length = _position - CurrentToken.StartPos;
            return CurrentToken;
        }

        if (char.IsDigit(Original[_position]))
        {
            CurrentToken = _tokenFactory.GetToken(TokenName.NumberToken, _position);
            SkipToEndOfNumber();
            CurrentToken.Length = _position - CurrentToken.StartPos;
            return CurrentToken;
        }

        CurrentToken = _tokenFactory.GetToken(TokenName.UndefinedToken, _position++);
        return CurrentToken;
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