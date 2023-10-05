namespace AParser.Parsers.Pg;

public interface IPgParser
{
    public string Parse(ITokenizer tokenizer, INode node);
}