

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace CreateMssTestDatabase.Entities;

[Table("ClientClaims")]
[DebuggerDisplay("{Type}: {Value}")]
public sealed class ClientClaim
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("Client")]
    public required string ClientId { get; set; }

    [StringLength(Constants.Data.UriLength)]
    public string? Issuer { get; set; }

    [StringLength(Constants.Data.UriLength)]
    public string? OriginalIssuer { get; set; }

    public ICollection<ClaimProperty>? Properties { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? Type { get; set; }

    [StringLength(Constants.Data.ValueLength)]
    public string? Value { get; set; }

    [StringLength(Constants.Data.NameLength)]
    public string? ValueType { get; set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ClientClaim)obj);
    }

    private bool Equals(ClientClaim other)
    {
        return Id == other.Id && 
               string.Equals(ClientId, other.ClientId, StringComparison.InvariantCulture) 
               && string.Equals(Issuer, other.Issuer, StringComparison.InvariantCulture) 
               && string.Equals(OriginalIssuer, other.OriginalIssuer, StringComparison.InvariantCulture) 
               && Equals(Properties, other.Properties) 
               && string.Equals(Type, other.Type, StringComparison.InvariantCulture) 
               && string.Equals(Value, other.Value, StringComparison.InvariantCulture) 
               && string.Equals(ValueType, other.ValueType, StringComparison.InvariantCulture);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Id;
            hashCode = (hashCode * 397) ^ StringComparer.InvariantCulture.GetHashCode(ClientId);
            hashCode = (hashCode * 397) ^ (Issuer != null ? StringComparer.InvariantCulture.GetHashCode(Issuer) : 0);
            hashCode = (hashCode * 397) ^ (OriginalIssuer != null ? StringComparer.InvariantCulture.GetHashCode(OriginalIssuer) : 0);
            hashCode = (hashCode * 397) ^ (Properties != null ? Properties.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Type != null ? StringComparer.InvariantCulture.GetHashCode(Type) : 0);
            hashCode = (hashCode * 397) ^ (Value != null ? StringComparer.InvariantCulture.GetHashCode(Value) : 0);
            hashCode = (hashCode * 397) ^ (ValueType != null ? StringComparer.InvariantCulture.GetHashCode(ValueType) : 0);
            return hashCode;
        }
    }
}