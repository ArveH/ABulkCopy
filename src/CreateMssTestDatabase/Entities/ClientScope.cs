using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CreateMssTestDatabase.Entities;

[ExcludeFromCodeCoverage]
[DebuggerDisplay("{ClientId}, {ScopeId}")]
public class ClientScope
{
    public required string ClientId { get; set; }
    [JsonIgnore]
    public Client? Client { get; set; }
    public required string ScopeId { get; set; }
    [JsonIgnore]
    public Scope? Scope { get; set; }
}