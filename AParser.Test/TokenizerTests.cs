namespace AParser.Test;

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
    [InlineData("(",  TokenName.LeftParenthesesToken)]
    [InlineData(")",  TokenName.RightParenthesesToken)]
    [InlineData("]",  TokenName.UndefinedToken)]
    [InlineData("1",  TokenName.NumberToken)]
    [InlineData("123",  TokenName.NumberToken)]
    [InlineData(",",  TokenName.CommaToken)]
    [InlineData("a",  TokenName.NameToken)]
    [InlineData("abc",  TokenName.NameToken)]
    [InlineData("_a",  TokenName.NameToken)]
    [InlineData("a_0",  TokenName.NameToken)]
    [InlineData("Aa0",  TokenName.NameToken)]
    [InlineData("a099",  TokenName.NameToken)]
    [InlineData("_", TokenName.NameToken)]
    [InlineData("0aA",  TokenName.NumberToken)]
    [InlineData(".90",  TokenName.UndefinedToken)]
    [InlineData("[arve]",  TokenName.QuotedNameToken)]
    [InlineData("[.90]",  TokenName.QuotedNameToken)]
    [InlineData("[ arve]",  TokenName.QuotedNameToken)]
    public void TestGetNext(string input, TokenName expected)
    {
        ITokenizer tokenizer = new Tokenizer(new TokenFactory());
        tokenizer.Initialize(input);

        var result = tokenizer.GetNext();

        result.Name.Should().Be(expected, $"because \"{input}\" should produce {expected}");
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
}