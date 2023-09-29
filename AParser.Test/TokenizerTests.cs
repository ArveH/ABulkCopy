namespace AParser.Test;

public class TokenizerTests
{
    [Fact]
    public void TestInitialize()
    {
        ITokenizer tokenizer = new Tokenizer();
        
        tokenizer.Initialize("some string");

        tokenizer.Original.Should().Be("some string", "because \"some string\" was passed to Initialize");
    }
}