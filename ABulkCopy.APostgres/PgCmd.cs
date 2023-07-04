namespace ABulkCopy.APostgres;

public class PgCmd : IPgCmd
{
    private readonly IPgContext _pgContext;

    public PgCmd(IPgContext pgContext)
    {
        _pgContext = pgContext;
    }

    public async Task DropTable(string tableName)
    {
        var sqlString = $"drop table if exists \"{tableName}\";";
        await ExecuteNonQuery(sqlString);
    }

    public async Task CreateTable(TableDefinition tableDefinition, bool addIfNotExists = false)
    {
        var sb = new StringBuilder();
        sb.Append("create table ");
        if (addIfNotExists) sb.Append("if not exists ");
        sb.AppendLine($"\"{tableDefinition.Header.Name}\" (");
        var first = true;
        foreach (var column in tableDefinition.Columns)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                sb.AppendLine(",");
            }

            sb.Append($"    \"{column.Name}\" {column.GetNativeCreateClause()}");
        }
        sb.AppendLine(");");
        await ExecuteNonQuery(sb.ToString());
    }

    public async Task CreateIndex(string tableName, IndexDefinition indexDefinition)
    {
        var sb = new StringBuilder();
        sb.Append("create ");
        if (indexDefinition.Header.IsUnique) sb.Append("unique ");
        sb.Append("index ");
        sb.AppendLine($"\"{indexDefinition.Header.Name}\" ");
        sb.Append("on ");
        sb.AppendLine($"\"{tableName}\" (");
        var first = true;
        foreach (var column in indexDefinition.Columns.Where(c => !c.IsIncluded))
        {
            if (first)
            {
                first = false;
            }
            else
            {
                sb.AppendLine(",");
            }

            sb.Append($"    \"{column.Name}\" ");
            if (column.Direction == Direction.Descending) sb.Append("desc ");
        }
        sb.AppendLine(")");
        if (indexDefinition.Columns.Any(c => c.IsIncluded))
        {
            sb.AppendLine(" include (");
            first = true;
            foreach (var column in indexDefinition.Columns.Where(c => c.IsIncluded))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.AppendLine(",");
                }

                sb.Append($"    \"{column.Name}\" ");
            }
            sb.Append(")");
        }

        await ExecuteNonQuery(sb.ToString());
    }

    public async Task ResetIdentity(string tableName, string columnName)
    {
        var seqName = $"{tableName}_{columnName}_seq";
        var oid = await SelectScalar($"select oid from pg_class where relkind = 'S' and relname = '{seqName}'");
        if (oid == null || oid == DBNull.Value)
        {
            throw new SqlNullValueException($"Sequence {seqName} not found");
        }

        var sb = new StringBuilder();
        sb.AppendLine("select setval(");
        sb.AppendLine($"{oid}, ");
        sb.AppendLine($"(select max(\"{columnName}\") from \"{tableName}\") )");

        await using var cmd = _pgContext.DataSource.CreateCommand(sb.ToString());
        await cmd.ExecuteScalarAsync();
    }

    public async Task ExecuteNonQuery(string sqlString)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<object?> SelectScalar(string sqlString)
    {
        await using var cmd = _pgContext.DataSource.CreateCommand(sqlString);
        return await cmd.ExecuteScalarAsync();
    }
}