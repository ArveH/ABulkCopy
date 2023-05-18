namespace ASqlServer.Column.ColumnTypes;

public abstract class TemplateSqlServerColumn : IColumn
{
    protected TemplateSqlServerColumn(int id, string name, bool isNullable)
    {
        Id = id;
        Name = name;
        IsNullable = isNullable;
    }

    public int Id { get; set; }
    public required string Name { get; set; }
    public required ColumnType Type { get; set; }
    public bool IsNullable { get; set; }
    public int Length { get; set; }

    public int? Precision { get; set; }
    public int? Scale { get; set; }
    public string? Collation { get; set; }

    public Identity? Identity { get; set; }

    public string? ComputedDefinition { get; set; }
    public bool IsComputed => ComputedDefinition != null;

    public bool HasDefault => DefaultConstraint != null;
    public DefaultDefinition? DefaultConstraint { get; set; }

    public abstract string ToString(object value);
    public abstract string InternalTypeName();
    public abstract object ToInternalType(string value);
    public abstract Type GetDotNetType();
}