namespace AParser.Tests;

public class TokenizerTests
{
    [Fact]
    public void TestInitialize()
    {
        ITokenizer tokenizer = new Tokenizer(new TokenFactory());
        
        tokenizer.Initialize("some string");

        tokenizer.Original.Should().Be("some string", "because \"some string\" was passed to Initialize");
    }

    [Theory]
    [InlineData("(",  TokenType.LeftParenthesesToken)]
    [InlineData(")",  TokenType.RightParenthesesToken)]
    [InlineData("]",  TokenType.UndefinedToken)]
    [InlineData("1",  TokenType.NumberToken)]
    [InlineData("123",  TokenType.NumberToken)]
    [InlineData("-1",  TokenType.NumberToken)]
    [InlineData(",",  TokenType.CommaToken)]
    [InlineData("a",  TokenType.NameToken)]
    [InlineData("abc",  TokenType.NameToken)]
    [InlineData("_a",  TokenType.NameToken)]
    [InlineData("a_0",  TokenType.NameToken)]
    [InlineData("Aa0",  TokenType.NameToken)]
    [InlineData("a099",  TokenType.NameToken)]
    [InlineData("_", TokenType.NameToken)]
    [InlineData(".90",  TokenType.NumberToken)]
    [InlineData("[arve]",  TokenType.QuotedNameToken)]
    [InlineData("[.90]",  TokenType.QuotedNameToken)]
    [InlineData("[ arve]",  TokenType.QuotedNameToken)]
    [InlineData("'arve'",  TokenType.StringToken)]
    [InlineData("N'arve'",  TokenType.NStringToken)]
    [InlineData("N 'arve'",  TokenType.NameToken)] // NOTE: The space after N means that we have two tokens
    public void TestGetNext(string input, TokenType expected)
    {
        ITokenizer tokenizer = new Tokenizer(new TokenFactory());
        tokenizer.Initialize(input);

        var result = tokenizer.GetNext();

        result.Type.Should().Be(expected, $"because \"{input}\" should produce {expected}");

        if (result.Type == TokenType.NumberToken)
        {
            result.Length.Should().Be(input.Length, $"because \"{input}\" has a length of {input.Length}");
        }
    }

    [Fact]
    public void TestGetNext_When_EmptyString()
    {
        ITokenizer tokenizer = new Tokenizer(new TokenFactory());
        
        var action = () => tokenizer.Initialize("");

        action.Should().Throw<AParserException>().WithMessage(ErrorMessages.EmptySql);
    }

    [Fact]
    public void TestGetNext_When_NoEndQuote()
    {
        ITokenizer tokenizer = new Tokenizer(new TokenFactory());
        tokenizer.Initialize("[arve");

        var action = () => tokenizer.GetNext();

        action.Should().Throw<UnclosedException>().WithMessage(ErrorMessages.Unclosed(']'));
    }

    [Fact]
    public void TestGetNext_When_EmptyQuote()
    {
        ITokenizer tokenizer = new Tokenizer(new TokenFactory());
        tokenizer.Initialize("[]");

        var action = () => tokenizer.GetNext();

        action.Should().Throw<TokenException>().WithMessage(ErrorMessages.EmptyQuote);
    }
}