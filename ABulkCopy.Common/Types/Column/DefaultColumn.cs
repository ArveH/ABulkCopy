namespace ABulkCopy.Common.Types.Column;

public class DefaultColumn : IColumn
{
    public DefaultColumn(int id, string name, bool isNullable)
    {
        Id = id;
        Name = name;
        IsNullable = isNullable;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public ColumnType Type { get; set; }
    public bool IsComputed => ComputedDefinition != null;
    public string? ComputedDefinition { get; set; }
    public bool IsNullable { get; set; }
    public Identity? Identity { get; set; }
    public int Length { get; set; }
    public int? Precision { get; set; }
    public int? Scale { get; set; }
    public bool HasDefault => DefaultConstraint != null;
    public DefaultDefinition? DefaultConstraint { get; set; }
    public string? Collation { get; set; }

    public virtual string ToString(object value)
    {
        throw new NotImplementedException();
    }

    public virtual string InternalTypeName()
    {
        throw new NotImplementedException();
    }

    public virtual object ToInternalType(string value)
    {
        throw new NotImplementedException();
    }

    public virtual Type GetDotNetType()
    {
        throw new NotImplementedException();
    }

    public virtual IColumn Clone()
    {
        return new DefaultColumn(Id, Name, IsNullable)
        {
            IsNullable = IsNullable,
            Collation = Collation,
            ComputedDefinition = ComputedDefinition,
            DefaultConstraint = DefaultConstraint?.Clone(),
            Identity = Identity?.Clone(),
            Length = Length,
            Precision = Precision,
            Scale = Scale
        };
    }
}