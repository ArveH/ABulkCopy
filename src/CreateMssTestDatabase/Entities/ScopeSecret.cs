namespace CreateMssTestDatabase.Entities;

[Table("ScopeSecrets")]
public class ScopeSecret : Secret
{
    [Required]
    [ForeignKey("Scope")]
    public required string ScopeId { get; set; }
}