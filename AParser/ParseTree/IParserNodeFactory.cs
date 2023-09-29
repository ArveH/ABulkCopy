namespace AParser.ParseTree;

public interface IParserNodeFactory
{
    IParserNode CreateNode(NodeType nodeType, ITokenizer tokenizer);
}