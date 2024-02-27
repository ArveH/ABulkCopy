// ReSharper disable NonReadonlyMemberInGetHashCode
namespace CreateMssTestDatabase.Entities;

public class DomainUriBase
{
    public virtual int Id { get; set; }

    public virtual required string ClientId { get; set; }

    public virtual string? Uri { get; set; }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((DomainUriBase)obj);
    }

    protected bool Equals(DomainUriBase other)
    {
        return Id == other.Id && string.Equals(ClientId, other.ClientId) && string.Equals(Uri, other.Uri);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Id;
            hashCode = (hashCode * 397) ^ ClientId.GetHashCode();
            hashCode = (hashCode * 397) ^ (Uri != null ? Uri.GetHashCode() : 0);
            return hashCode;
        }
    }
}