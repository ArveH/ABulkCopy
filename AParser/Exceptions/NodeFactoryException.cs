namespace AParser.Exceptions;

public class NodeFactoryException : Exception
{
    public NodeFactoryException(NodeType nodeType)
        : base(ErrorMessages.CreateNode(nodeType))
    {
    }
}