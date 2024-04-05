namespace ABulkCopy.Cmd.Internal;

public interface ICopyIn
{
    Task RunAsync(Rdbms rdbms, CancellationToken ctsToken);
}