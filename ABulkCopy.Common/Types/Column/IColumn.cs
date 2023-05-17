namespace ABulkCopy.Common.Types.Column;

public interface IColumn
{
    int Id { get; set; }
    string Name { get; set; }
    ColumnType Type { get; set; }
    bool IsComputed { get; }
    string? ComputedDefinition { get; set; }
    bool IsNullable { get; set; }
    Identity? Identity { get; set; }
    int Length { get; set; }
    int? Precision { get; set; }
    int? Scale { get; set; }
    bool HasDefault { get; }
    DefaultDefinition? DefaultConstraint { get; set; }
    string? Collation { get; set; }

    string ToString(object value);
    string InternalTypeName();
    object ToInternalType(string value);
    Type GetDotNetType();
}