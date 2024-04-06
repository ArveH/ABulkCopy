namespace ABulkCopy.Cmd.Internal;

public interface ICopyOut
{
    Task RunAsync(CancellationToken ct);
}