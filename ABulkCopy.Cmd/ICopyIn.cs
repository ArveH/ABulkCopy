namespace ABulkCopy.Cmd;

internal interface ICopyIn
{
    Task Run(string folder, Rdbms contextRdbms);
}