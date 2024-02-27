

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace CreateMssTestDatabase.Entities;

[Table("ExternalUserClaims")]
[DebuggerDisplay("{Type}: {Value}")]
public sealed class ExternalUserClaim
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("ExternalUserProfile")]
    public required string ExternalUserProfileId { get; set; } // ExternalUserProfile.SubjectId

    [StringLength(Constants.Data.NameLength)]
    public string? Type { get; set; }

    [StringLength(Constants.Data.ValueLength)]
    public string? Value { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string ValueType { get; set; } = "http://www.w3.org/2001/XMLSchema#string";

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ExternalUserClaim)obj);
    }

    private bool Equals(ExternalUserClaim other)
    {
        return Id == other.Id &&
               string.Equals(ExternalUserProfileId, other.ExternalUserProfileId, StringComparison.InvariantCulture)
               && string.Equals(Type, other.Type, StringComparison.InvariantCulture)
               && string.Equals(Value, other.Value, StringComparison.InvariantCulture)
               && string.Equals(ValueType, other.ValueType, StringComparison.InvariantCulture);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Id;
            hashCode = (hashCode * 397) ^ StringComparer.InvariantCulture.GetHashCode(ExternalUserProfileId);
            hashCode = (hashCode * 397) ^ (Type != null ? StringComparer.InvariantCulture.GetHashCode(Type) : 0);
            hashCode = (hashCode * 397) ^ (Value != null ? StringComparer.InvariantCulture.GetHashCode(Value) : 0);
            hashCode = (hashCode * 397) ^ StringComparer.InvariantCulture.GetHashCode(ValueType);
            return hashCode;
        }
    }

}