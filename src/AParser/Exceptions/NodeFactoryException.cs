namespace AParser.Exceptions;

public class NodeFactoryException : Exception
{
    public NodeFactoryException(NodeType type)
        : base(ErrorMessages.CreateNode(type))
    {
    }
}