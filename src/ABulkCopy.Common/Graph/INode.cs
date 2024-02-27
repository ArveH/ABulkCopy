namespace ABulkCopy.Common.Graph;

public interface INode
{
    string Name { get; }
    TableDefinition? TableDefinition { get; }
    bool IsRoot { get; }
    bool IsIndependent { get; }
    public Dictionary<string, INode> Parents { get; }
    public Dictionary<string, INode> Children { get; }
    void Accept(IVisitor visitor, int depth);
}