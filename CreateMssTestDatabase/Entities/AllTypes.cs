namespace CreateMssTestDatabase.Entities;

public class AllTypes
{
    public long Id { get; set; }

    [Column(TypeName = "bigint")]
    public long ExactNumBigInt { get; set; }
    [Column(TypeName = "int")]
    public int ExactNumInt { get; set; }
    [Column(TypeName = "smallint")]
    public int ExactNumSmallInt { get; set; }
    [Column(TypeName = "tinyint")]
    public int ExactNumTinyInt { get; set; }
    [Column(TypeName = "bit")]
    public bool ExactNumBit { get; set; }
    [Column(TypeName = "money")]
    public decimal ExactNumMoney { get; set; }
    [Column(TypeName = "smallmoney")]
    public decimal ExactNumSmallMoney { get; set; }
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
    [Column(TypeName = "datetime")]
    public DateTime DTDateTime { get; set; }
    [Column(TypeName = "datetime2")]
    public DateTime DTDateTime2 { get; set; }
    [Column(TypeName = "smalldatetime")]
    public DateTime DTSmallDateTime { get; set; }
    [Column(TypeName = "datetimeoffset")]
    public DateTimeOffset DTDateTimeOffset { get; set; }
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
    [Column(TypeName = "varchar(max)")]
    [Unicode(false)]
    public string? CharStrVarchar10K { get; set; }
    [Column(TypeName = "nchar(20)")]
    [MaxLength(20)]
    public string? CharStrNChar20 { get; set; }
    [Column(TypeName = "nvarchar(20)")]
    [MaxLength(20)]
    public string? CharStrNVarchar20 { get; set; }
    [Column(TypeName = "nvarchar(max)")]
    public string? CharStrNVarchar10K { get; set; }
    [Column(TypeName = "binary")]
    [MaxLength(5000)]
    public byte[]? BinBinary5K { get; set; }
    [Column(TypeName = "varbinary(max)")]
    public byte[]? BinVarbinary10K { get; set; }
    [Column(TypeName = "uniqueidentifier")]
    public Guid OtherGuid { get; set; }
    [Column(TypeName = "xml")]
    public string? OtherXml { get; set; }

    public AllTypes Copy()
    {
        return (AllTypes)MemberwiseClone();
    }
}