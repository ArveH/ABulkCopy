namespace ABulkCopy.Common.Utils;

public interface ISelectCreator
{
    string CreateSelect(TableDefinition tableDefinition);
}