namespace ABulkCopy.Cmd;

public interface ICopyIn
{
    Task RunAsync(Rdbms rdbms, CancellationToken ctsToken);
}