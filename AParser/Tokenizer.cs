namespace AParser;

public class Tokenizer : ITokenizer
{
    private const char QuoteStartChar = '[';
    private const char QuoteEndChar = ']';

    private readonly ITokenFactory _tokenFactory;
    private string? _original;
    private int _position;

    private char CurrentChar => _position >= Original.Length ? '\0' : Original[_position];
    private char PeekNextChar => _position + 1 >= Original.Length ? '\0' : Original[_position + 1];

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

    public IToken GetExpected(TokenType expectedToken)
    {
        var token = GetNext();
        if (token.Type != expectedToken)
        {
            throw new UnexpectedTokenException(expectedToken, token.Type);
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
                CurrentToken = _tokenFactory.GetToken(TokenType.LeftParenthesesToken, _position++);
                return CurrentToken;
            case ')':
                CurrentToken = _tokenFactory.GetToken(TokenType.RightParenthesesToken, _position++);
                return CurrentToken;
            case ',':
                CurrentToken = _tokenFactory.GetToken(TokenType.CommaToken, _position++);
                return CurrentToken;
        }

        if (IsQuotedNameStartingChar(CurrentChar))
        {
            CurrentToken = _tokenFactory.GetToken(TokenType.QuotedNameToken, _position);
            _position++;
            SkipToEndOfQuotedName();
            _position++;
            if (_position == CurrentToken.StartPos + 2)
            {
                throw new TokenException(ErrorMessages.EmptyQuote);
            }

            CurrentToken.Length = _position - CurrentToken.StartPos;
            return CurrentToken;
        }

        if (IsNameStartingChar(CurrentChar))
        {
            CurrentToken = _tokenFactory.GetToken(TokenType.NameToken, _position);
            SkipToEndOfName();
            CurrentToken.Length = _position - CurrentToken.StartPos;
            return CurrentToken;
        }

        if (char.IsDigit(CurrentChar))
        {
            CurrentToken = _tokenFactory.GetToken(TokenType.NumberToken, _position);
            SkipToEndOfNumber();
            CurrentToken.Length = _position - CurrentToken.StartPos;
            return CurrentToken;
        }

        if (CurrentChar == '.' && char.IsDigit(PeekNextChar))
        {
            CurrentToken = _tokenFactory.GetToken(TokenType.NumberToken, _position);
            SkipToEndOfNumber();
            CurrentToken.Length = _position - CurrentToken.StartPos;
            return CurrentToken;
        }

        CurrentToken = _tokenFactory.GetToken(TokenType.UndefinedToken, _position++);
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

    private void SkipToEndOfQuotedName()
    {
        while (_position < Original.Length && CurrentChar != QuoteEndChar)
        {
            _position++;
        }

        if (CurrentChar != QuoteEndChar)
        {
            throw new UnclosedException(QuoteEndChar);
        }
    }

    private void SkipToEndOfNumber()
    {
        while (char.IsDigit(CurrentChar))
        {
            _position++;
        }

        if (CurrentChar != '.')
        {
            return;
        }

        _position++;
        while (char.IsDigit(CurrentChar))
        {
            _position++;
        }
    }

    private bool IsQuotedNameStartingChar(char c)
    {
        return c == QuoteStartChar;
    }

    private bool IsNameStartingChar(char c)
    {
        return char.IsLetter(c) || c == '_';
    }

    private bool IsNumberChar(char c)
    {
        return char.IsDigit(c) || c == '.';
    }

    private bool IsNameChar(char c)
    {
        return char.IsLetterOrDigit(c) || c == '_';
    }
}