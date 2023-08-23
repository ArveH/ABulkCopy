namespace ABulkCopy.Cmd;

public interface ICopyOut
{
    Task Run(CmdArguments cmdArguments);
}