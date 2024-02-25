using System.Diagnostics.CodeAnalysis;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace CreateMssTestDatabase.Entities;

[ExcludeFromCodeCoverage]
[Table("ClaimProperties")]
[DebuggerDisplay("{Key}: {Value}")]
public sealed class ClaimProperty
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("ClientClaim")]
    public int ClientClaimId { get; set; }

    [Required]
    [StringLength(Constants.Data.NameLength)]
    public required string Key { get; set; }

    [StringLength(Constants.Data.ValueLength)]
    public string? Value { get; set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ClaimProperty) obj);
    }

    private bool Equals(ClaimProperty other)
    {
        return Id == other.Id && 
               ClientClaimId == other.ClientClaimId 
               && string.Equals(Key, other.Key, StringComparison.InvariantCulture) 
               && string.Equals(Value, other.Value, StringComparison.InvariantCulture);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Id;
            hashCode = (hashCode * 397) ^ ClientClaimId;
            hashCode = (hashCode * 397) ^ StringComparer.InvariantCulture.GetHashCode(Key);
            hashCode = (hashCode * 397) ^ (Value != null ? StringComparer.InvariantCulture.GetHashCode(Value) : 0);
            return hashCode;
        }
    }
}