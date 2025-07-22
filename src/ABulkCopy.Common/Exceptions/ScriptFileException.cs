namespace ABulkCopy.Common.Exceptions;

public class ScriptFileException : Exception
{
    public ScriptFileException(int row, string message) : base($"Error on row {row}: {message}")
    {
        Row = row;
    }
    
    public int Row { get; }
}