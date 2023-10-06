namespace AParser.Parsers.Pg;

public class PgParser : IPgParser
{
    public string Parse(ITokenizer tokenizer, INode node)
    {
        switch (node.Type)
        {
            case NodeType.CommaNode:
            case NodeType.LeftParenthesesNode:
            case NodeType.NameNode:
            case NodeType.NumberNode:
            case NodeType.QuotedNameNode:
            case NodeType.RightParenthesesNode:
            case NodeType.TypeNode:
                return ParseLeafNode(tokenizer, node);
            default:
                return ParseExpression(tokenizer, node);
        }
    }

    public string ParseExpression(ITokenizer tokenizer, INode node)
    {
        switch (node.Type)
        {
            case NodeType.ConvertFunctionNode:
                return ParseConvertFunctionNode(tokenizer, node);
            case NodeType.NumberNode:
                return ParseLeafNode(tokenizer, node);
            case NodeType.ParenthesesNode:
                return ParseParenthesesNode(tokenizer, node);
            default:
                throw new AParserException(ErrorMessages.UnexpectedNode(node.Type));
        }
    }

    public string ParseConvertFunctionNode(ITokenizer tokenizer, INode node)
    {
        var sqlType = tokenizer.GetSpan(node.Children[2].Tokens.First())
            .ToString().ToLower();
        string pgConvertFunction;
        switch (sqlType)
        {
            case "bit":
            case "[bit]":
                pgConvertFunction = "to_number";
                break;
            default:
                throw new UnknownSqlTypeException(ErrorMessages.UnknownSqlType(sqlType));
        }

        return pgConvertFunction + 
               ParseLeafNode(tokenizer, node.Children[1]) +
               ParseExpression(tokenizer, node.Children[4]) +
               ParseLeafNode(tokenizer, node.Children[5]);
    }

    public string ParseParenthesesNode(ITokenizer tokenizer, INode node)
    {
        return ParseLeafNode(tokenizer, node.Children[0]) + 
               Parse(tokenizer, node.Children[1]) + 
               ParseLeafNode(tokenizer, node.Children[2]);
    }

    public static string ParseLeafNode(ITokenizer tokenizer, INode node)
    {
        return tokenizer.GetSpan(node.Tokens.First()).ToString();
    }
}