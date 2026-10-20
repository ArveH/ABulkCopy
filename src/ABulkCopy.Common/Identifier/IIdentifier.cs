namespace ABulkCopy.Common.Identifier;

public interface IIdentifier
{
    string Get(string name);
    string Get(string name, bool isCaseSensitive);
    string AdjustForSystemTable(string name);
}