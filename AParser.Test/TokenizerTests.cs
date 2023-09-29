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
    [InlineData("",  TokenName.EofToken)]
    [InlineData("(",  TokenName.LeftParenthesesToken)]
    [InlineData(")",  TokenName.RightParenthesesToken)]
    [InlineData("[",  TokenName.SquareLeftParenthesesToken)]
    [InlineData("]",  TokenName.SquareRightParenthesesToken)]
    [InlineData("1",  TokenName.NumberToken)]
    [InlineData("123",  TokenName.NumberToken)]
    [InlineData(",",  TokenName.CommaToken)]
    [InlineData("a",  TokenName.NameToken)]
    [InlineData("abc",  TokenName.NameToken)]
    [InlineData("_a",  TokenName.NameToken)]
    [InlineData("a_0",  TokenName.NameToken)]
    [InlineData("Aa0",  TokenName.NameToken)]
    [InlineData("a099",  TokenName.NameToken)]
    [InlineData("_", TokenName.UndefinedToken)]
    [InlineData("0aA",  TokenName.UndefinedToken)]
    [InlineData(".90",  TokenName.UndefinedToken)]
    public void TestGetNext_Then_TokenNameIsCorrect(string input, TokenName expected)
    {
        ITokenizer tokenizer = new Tokenizer(new TokenFactory());
        tokenizer.Initialize(input);

        var result = tokenizer.GetNext();

        result.Name.Should().Be(expected, $"because \"{input}\" should produce {expected}");
    }

}