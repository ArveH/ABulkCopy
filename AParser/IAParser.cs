namespace AParser;

public interface IAParser
{
    void ParseExpression(ITokenizer tokenizer, IParseTree parseTree);
    void ParseFunction(ITokenizer tokenizer, IParseTree parseTree);
    void ParseConvertFunction(ITokenizer tokenizer, IParseTree parseTree);
    void ParseParentheses(ITokenizer tokenizer, IParseTree parseTree);
    void ParseNumber(ITokenizer tokenizer, IParseTree parseTree);
    void ParseQuotedName(ITokenizer tokenizer, IParseTree parseTree);
    void ParseType(ITokenizer tokenizer, IParseTree parseTree);
    void ParseName(ITokenizer tokenizer, IParseTree parseTree);
}