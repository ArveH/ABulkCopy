namespace ABulkCopy.Cmd;

public interface ICopyOut
{
    Task RunAsync(CancellationToken ct);
}