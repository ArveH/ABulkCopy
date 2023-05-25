namespace ABulkCopy.Cmd;

public interface ICopyOut
{
    Task Run(string folder, string searchStr);
}