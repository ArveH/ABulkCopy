namespace AParser.Tree;

public interface INode
{
    public NodeType NodeType { get; }
    public T Create<T>();
}