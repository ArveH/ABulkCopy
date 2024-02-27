namespace AParser;

public class TokenizerFactory : ITokenizerFactory
{
    private readonly ITokenFactory _tokenFactory;

    public TokenizerFactory(ITokenFactory tokenFactory)
    {
        _tokenFactory = tokenFactory;
    }

    public ITokenizer GetTokenizer()
    {
        return new Tokenizer(_tokenFactory);
    }
}