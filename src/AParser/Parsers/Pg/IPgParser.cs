namespace AParser.Parsers.Pg;

public interface IPgParser
{
    string Parse(ITokenizer tokenizer, INode node);
    string ParseExpression(ITokenizer tokenizer, INode node);
    string ParseConvertFunction(ITokenizer tokenizer, INode node);
    string ParseConvertToNumber(ITokenizer tokenizer, INode node);
    string ParseTodayFunction(ITokenizer tokenizer, INode node);
    string ParseGuidFunction(ITokenizer tokenizer, INode node);
    string ParseParentheses(ITokenizer tokenizer, INode node);
    string ParseLeafNode(ITokenizer tokenizer, INode node);
}