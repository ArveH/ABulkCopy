namespace AParser.Tests;

public class ParserTestBase
{
    protected ITokenizer GetTokenizer()
    {
        return new Tokenizer(new TokenFactory());
    }

    protected IParseTree GetParseTree()
    {
        return new ParseTree(new NodeFactory(), new SqlTypes());
    }

}