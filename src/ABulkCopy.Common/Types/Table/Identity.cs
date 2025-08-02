namespace ABulkCopy.Common.Types.Table;

public class Identity
{
    public Identity(string? name=null, int seed=1, int increment=1, char type = 'a')
    {
        if (type != 'a' && type != 'd')
            throw new ArgumentException("Type must be 'a' or 'd'", nameof(type));
        ColumnName = name;
        Seed = seed;
        Increment = increment;
        Type = type;
    }
    public string? ColumnName { get; set; }
    public int Seed { get; set; }
    public int Increment { get; set; }
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
