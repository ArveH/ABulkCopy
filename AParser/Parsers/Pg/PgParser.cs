namespace AParser.Parsers.Pg;

public class PgParser : IPgParser
{
    public string Parse(ITokenizer tokenizer, INode node)
    {
        if (node.Type == NodeType.ConvertFunctionNode)
        {
            return "TODO: Cast";
        }

        throw new AParserException(ErrorMessages.UnexpectedNode(node.Type));
    }
}