namespace AParser.Tree;

public class ParseTree : IParseTree
{
    private INode? _rootNode;
    public INode RootNode => _rootNode 
                             ?? throw new InvalidOperationException("RootNode is null");
}