namespace ABulkCopy.Common.Identifier;

public interface IIdentifier
{
    string Get(string name);
    string AdjustForSystemTable(string name);
}