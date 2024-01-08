namespace ABulkCopy.Cmd;

internal interface ICopyIn
{
    Task RunAsync(Rdbms rdbms);
}