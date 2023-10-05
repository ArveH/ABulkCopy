namespace AParser;

public interface IAParser
{
    INode ParseExpression(ITokenizer tokenizer);
    INode ParseFunction(ITokenizer tokenizer);
    INode ParseConvertFunction(ITokenizer tokenizer);
    INode ParseParentheses(ITokenizer tokenizer);
    INode ParseNumber(ITokenizer tokenizer);
    INode ParseType(ITokenizer tokenizer);
    INode ParseName(ITokenizer tokenizer);
}