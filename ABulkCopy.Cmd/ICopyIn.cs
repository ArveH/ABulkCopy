namespace ABulkCopy.Cmd;

internal interface ICopyIn
{
    Task Run(CmdArguments cmdArguments);
}