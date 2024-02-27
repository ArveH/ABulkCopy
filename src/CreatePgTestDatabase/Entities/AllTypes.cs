namespace CreatePgTestDatabase.Entities;

public class AllTypes
{
    public long Id { get; set; }

    [Column(TypeName = "bigint")]
    public long ExactNumBigInt { get; set; }
    [Column(TypeName = "int")]
    public int ExactNumInt { get; set; }
    [Column(TypeName = "smallint")]
    public int ExactNumSmallInt { get; set; }
    [Column(TypeName = "bool")]
    public bool ExactNumBool { get; set; }
    [Column(TypeName = "money")]
    public decimal ExactNumMoney { get; set; }
    [Column(TypeName = "decimal")]
    [Precision(28,3)]
    public decimal ExactNumDecimal { get; set; }
    [Column(TypeName = "numeric")]
    [Precision(28, 3)]
    public decimal ExactNumNumeric { get; set; }
    [Column(TypeName = "float")]
    public double ApproxNumFloat { get; set; }
    [Column(TypeName = "real")]
    public double ApproxNumReal { get; set; }
    [Column(TypeName = "date")]
    public DateTime DTDate { get; set; }
    [Column(TypeName = "timestamp")]
    public DateTime DTTimeStamp { get; set; }
    [Column(TypeName = "timestamptz")]
    public DateTimeOffset DTTimeStampTz { get; set; }
    [Column(TypeName = "time")]
    public TimeSpan DTTime { get; set; }
    [Column(TypeName = "char(20)")]
    [Unicode(false)]
    [MaxLength(20)]
    public string? CharStrChar20 { get; set; }
    [Column(TypeName = "varchar(20)")]
    [Unicode(false)]
    [MaxLength(20)]
    public string? CharStrVarchar20 { get; set; }
    [Column(TypeName = "varchar(10000)")]
    [Unicode(false)]
    public string? CharStrVarchar10K { get; set; }
    [Column(TypeName = "bytea")]
    [MaxLength(5000)]
    public byte[]? BinByteA5K { get; set; }
    [Column(TypeName = "bytea")]
    public byte[]? BinByteA10K { get; set; }
    [Column(TypeName = "uuid")]
    public Guid OtherUuid { get; set; }

    public AllTypes Copy()
    {
        return (AllTypes)MemberwiseClone();
    }
}