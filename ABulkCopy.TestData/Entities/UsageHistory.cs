using System.Diagnostics.CodeAnalysis;

namespace ABulkCopy.TestData.Entities;

[ExcludeFromCodeCoverage]
[DebuggerDisplay("{EntityId} ({EntityType})")]
public class UsageHistory
{
    public long Id { get; set; }

    [EnumDataType(typeof(TEntity))]
    public TEntity EntityType { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public required string EntityId { get; set; }

    public DateTime LastUsed { get; set; }
}