namespace ABulkCopy.Cmd;

public enum CmdStatus
{
    Ok = 1,
    Created = 2,
    Exists = 3,
    ShouldExit = -1
}