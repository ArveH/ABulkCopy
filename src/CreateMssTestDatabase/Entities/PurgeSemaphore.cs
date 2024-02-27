using System.Diagnostics.CodeAnalysis;

namespace CreateMssTestDatabase.Entities;

/// <summary>
/// This will be used as a flag, to check if a purge is running or not.
/// Before you start purging, you insert the value Constants.PurgeAudits.SemaphoreValue into this table.
/// If you can't (unique constraint violated), a purge is already running
/// </summary>
[ExcludeFromCodeCoverage]
public class PurgeSemaphore
{
    [Key]
    [StringLength(Constants.Data.NameLength)]
    public required string Value { get; set; }
}