namespace ABulkCopy.Common.Utils;

public interface IQueryBuilder
{
    void AppendIdentifier(string identifier);
    void AppendIdentifier(string identifier, bool isCaseSensitive);
    void Append(string str);
    void AppendLine(string str);
    string ToString();
    string CreateTableStmt(TableDefinition tableDefinition, bool addIfNotExists = false);
    string DropTableStmt(SchemaTableTuple st);
    string CreateIndexStmt(SchemaTableTuple st, IndexDefinition indexDefinition);
    void AppendIdentifierList(IEnumerable<string> names);
    void AppendColumns(TableDefinition tableDefinition);
}
