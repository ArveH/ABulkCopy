namespace AParser;

public interface IAParser
{
    INode ParseExpression(ITokenizer tokenizer, IParseTree parseTree);
    INode ParseFunction(ITokenizer tokenizer, IParseTree parseTree);
    INode ParseConvertFunction(ITokenizer tokenizer, IParseTree parseTree);
    INode ParseParentheses(ITokenizer tokenizer, IParseTree parseTree);
    INode ParseNumber(ITokenizer tokenizer, IParseTree parseTree);
    INode ParseType(ITokenizer tokenizer, IParseTree parseTree);
    INode ParseName(ITokenizer tokenizer, IParseTree parseTree);
}