namespace AParser;

public interface IAParser
{
    IEnumerable<IToken> ParseExpression(ITokenizer tokenizer, IParseTree parseTree);
    IEnumerable<IToken> ParseFunction(ITokenizer tokenizer, IParseTree parseTree);
    IEnumerable<IToken> ParseConvertFunction(ITokenizer tokenizer, IParseTree parseTree);
    IEnumerable<IToken> ParseParentheses(ITokenizer tokenizer, IParseTree parseTree);
    IEnumerable<IToken> ParseNumber(ITokenizer tokenizer, IParseTree parseTree);
    IEnumerable<IToken> ParseType(ITokenizer tokenizer, IParseTree parseTree);
    IEnumerable<IToken> ParseName(ITokenizer tokenizer, IParseTree parseTree);
}