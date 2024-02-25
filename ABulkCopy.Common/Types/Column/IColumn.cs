namespace ABulkCopy.Common.Types.Column;

public interface IColumn
{
    int Id { get; set; }
    string Name { get; set; }
    string Type { get; set; }
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
    string GetTypeClause();
    object ToInternalType(string value);
    string GetNullableClause();
    string GetIdentityClause();
    Type GetDotNetType();
}