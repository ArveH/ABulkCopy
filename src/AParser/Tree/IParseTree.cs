namespace AParser.Tree;

public interface IParseTree
{
    INode CreateExpression(ITokenizer tokenizer);
    INode CreateFunction(ITokenizer tokenizer);
    INode CreateConvertFunction(ITokenizer tokenizer);
    INode CreateParentheses(ITokenizer tokenizer);
    INode CreateNumber(ITokenizer tokenizer);
    INode CreateType(ITokenizer tokenizer);
    INode CreateName(ITokenizer tokenizer);
}