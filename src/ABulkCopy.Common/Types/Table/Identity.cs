namespace ABulkCopy.Common.Types.Table;

public class Identity
{
    public Identity(string? columnName=null, long seed=1, long increment=1, char type = 'a')
    {
        if (type != 'a' && type != 'd')
            throw new ArgumentException("Type must be 'a' or 'd'", nameof(type));
        ColumnName = columnName;
        Seed = seed;
        Increment = increment;
        Type = type;
    }
    public string? ColumnName { get; set; }
    public long Seed { get; set; }
    public long Increment { get; set; }
    private char _type = 'a';
    public char Type
    {
        get => _type;
        set
        {
            if (value != 'a' && value != 'd')
                throw new ArgumentException("Type must be 'a' or 'd'");
            _type = value;
        }
    }
    public Identity Clone()
    {
        return new Identity(ColumnName, Seed, Increment, Type);
    }
}
