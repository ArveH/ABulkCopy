namespace ABulkCopy.Common.Graph.Visitors;

public interface IToStringVisitor : IVisitor
{
    public string Result { get; }
}