namespace ABulkCopy.Common.Config;

public record InsertSettings
{
    public EmptyStringFlag EmptyStringFlag { get; set; } = EmptyStringFlag.Leave;
    public bool SkipCreate { get; set; }
    public bool SkipZeroByteInString { get; set; } = true;
}