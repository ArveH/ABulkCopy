namespace ABulkCopy.Common.Types.Table;

public class Identity
{
    public Identity(int seed=1, int increment=1)
    {
        Seed = seed;
        Increment = increment;
    }
    public int Seed { get; set; }
    public int Increment { get; set; }

    public Identity Clone()
    {
        return new Identity(Seed, Increment);
    }
}