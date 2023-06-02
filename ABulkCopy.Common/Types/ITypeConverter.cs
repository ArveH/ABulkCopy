namespace ABulkCopy.Common.Types;

public interface ITypeConverter
{
    TableDefinition Convert(TableDefinition sourceDefinition);
}