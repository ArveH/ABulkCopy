﻿namespace AParser.Parsers.Pg;

public class PgParser : IPgParser
{
    public ParserModifiers Modifiers { get; } = new();

    public string Parse(ITokenizer tokenizer, INode node)
    {
        switch (node.Type)
        {
            case NodeType.CommaNode:
            case NodeType.LeftParenthesesNode:
            case NodeType.NameNode:
            case NodeType.NStringNode:
            case NodeType.NumberNode:
            case NodeType.QuotedNameNode:
            case NodeType.RightParenthesesNode:
            case NodeType.StringNode:
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
                return ParseConvertFunction(tokenizer, node);
            case NodeType.GuidFunctionNode:
                return ParseGuidFunction(tokenizer, node);
            case NodeType.TodayFunctionNode:
                return ParseTodayFunction(tokenizer, node);
            case NodeType.NumberNode:
            case NodeType.NStringNode:
            case NodeType.StringNode:
                return ParseLeafNode(tokenizer, node);
            case NodeType.ParenthesesNode:
                return ParseParentheses(tokenizer, node);
            default:
                throw new AParserException(ErrorMessages.UnexpectedNode(node.Type));
        }
    }

    public string ParseConvertFunction(ITokenizer tokenizer, INode node)
    {
        var sqlType = tokenizer.GetSpan(node.Children[2].Tokens.First())
            .ToString().ToLower();
        return sqlType switch
        {
            "bit" => ParseConvertBit(tokenizer, node),
            "[bit]" => ParseConvertBit(tokenizer, node),
            "datetime" => ParseConvertToTimestamp(tokenizer, node),
            "[datetime]" => ParseConvertToTimestamp(tokenizer, node),
            "smalldatetime" => ParseConvertToTimestamp(tokenizer, node),
            "[smalldatetime]" => ParseConvertToTimestamp(tokenizer, node),
            _ => throw new UnknownSqlTypeException(ErrorMessages.UnknownSqlType(sqlType))
        };
    }

    private string ParseConvertToTimestamp(ITokenizer tokenizer, INode node)
    {
        var dateToken = node.Children[4].Tokens.FirstOrDefault();
        if (dateToken?.Type != TokenType.StringToken && dateToken?.Type != TokenType.NStringToken)
        {
            return "cast(" +
                ParseExpression(tokenizer, node.Children[4]) +
                " as timestamp)";
        }

        var dateStr = tokenizer.GetUnquotedSpan(dateToken).ToString();

        var longDate = dateStr.ExtractLongDateString();
        if (longDate == null)
        {
            // CAST doesn't accept strings with milliseconds
            return $"cast('{dateStr.Replace(":000", "")}' as timestamp)";
        }
        return $"to_timestamp('{longDate}', 'YYYYMMDD HH24:MI:SS:FF3')";

    }

    public string ParseConvertBit(ITokenizer tokenizer, INode node)
    {
        var convertTo = Modifiers.ConvertBitToBoolean
            ? "::boolean"
            : "::integer";

        return ParseExpression(tokenizer, node.Children[4]) + convertTo;
    }

    public string ParseConvertToNumber(ITokenizer tokenizer, INode node)
    {
        return ParseExpression(tokenizer, node.Children[4]) +
               "::integer";
    }

    public string ParseTodayFunction(ITokenizer tokenizer, INode node)
    {
        return "localtimestamp";
    }

    public string ParseGuidFunction(ITokenizer tokenizer, INode node)
    {
        return "gen_random_uuid()";
    }

    public string ParseParentheses(ITokenizer tokenizer, INode node)
    {
        return "(" + 
               Parse(tokenizer, node.Children[1]) + 
               ")";
    }

    public string ParseLeafNode(ITokenizer tokenizer, INode node)
    {
        return tokenizer.GetSpan(node.Tokens.First()).ToString();
    }
}