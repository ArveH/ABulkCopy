namespace ABulkCopy.Common.Types.Column;

public class DefaultColumn : IColumn
{
    public DefaultColumn(int id, string type, string name, bool isNullable)
    {
        Id = id;
        Type = type;
        Name = name;
        IsNullable = isNullable;
    }

    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    [JsonIgnore]
    public bool IsComputed => ComputedDefinition != null;
    public bool IsNullable { get; set; }
    public Identity? Identity { get; set; }
    public string? ComputedDefinition { get; set; }
    public int Length { get; set; }
    public int? Precision { get; set; }
    public int? Scale { get; set; }
    [JsonIgnore]
    public bool HasDefault => DefaultConstraint != null;
    public DefaultDefinition? DefaultConstraint { get; set; }
    public string? Collation { get; set; }

    public virtual string ToString(object value)
    {
        throw new NotImplementedException();
    }

    public virtual string GetTypeClause()
    {
        return Type;
    }

    public virtual string GetNativeCreateClause()
    {
        return GetTypeClause() + GetIdentityClause() + GetDefaultClause() + GetNullableClause();
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
        return new DefaultColumn(Id, Type, Name, IsNullable)
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

    protected string GetNullableClause()
    {
        return IsNullable ? " NULL" : " NOT NULL";
    }

    protected virtual string GetIdentityClause() {
        throw new NotImplementedException();
    }

    protected virtual string GetDefaultClause()
    {
        return DefaultConstraint == null ? " " : $" DEFAULT {DefaultConstraint.Definition}";
    }
}