namespace AParser.ParseTree;

public class ExpressionNode : ParserNode
{
    private readonly IParserNodeFactory _parserNodeFactory;

    public ExpressionNode(IParserNodeFactory parserNodeFactory)
    {
        _parserNodeFactory = parserNodeFactory;
    }

    public override void Create(ITokenizer tokenizer)
    {
        var token = tokenizer.GetNext();
        
        if (token.Name == TokenName.LeftParenthesesToken)
        {
            Children.Add(_parserNodeFactory.CreateNode(NodeType.ParenthesesNode, tokenizer));
            return;
        }

        throw new CreateNodeException($"Unexpected token: {token.Name}");
    }
}