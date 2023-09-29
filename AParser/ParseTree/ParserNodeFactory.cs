namespace AParser.ParseTree;

public class ParserNodeFactory : IParserNodeFactory
{
    public IParserNode CreateNode(NodeType nodeType, ITokenizer tokenizer)
    {
        IParserNode node = nodeType switch
        {
            NodeType.ExpressionNode => new ExpressionNode(this),
            NodeType.ParenthesesNode => new ParenthesesNode(),
            _ => throw new Exception($"Unknown node type: {nodeType}")
        };

        node.Create(tokenizer);
        return node;
    }
}