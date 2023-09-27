namespace ABulkCopy.Cmd;

internal interface ICopyIn
{
    Task Run(Rdbms rdbms);
}