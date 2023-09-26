namespace ABulkCopy.Common.Utils;

public interface IQueryBuilder
{
    void AppendIdentifier(string identifier);
    void Append(string str);
    void AppendLine(string str);
    string ToString();
    string CreateDropTableStmt(string tableName);
    void AppendIdentifierList(IEnumerable<string> names);
    void AppendColumnNames(TableDefinition tableDefinition);
}
