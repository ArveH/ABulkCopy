namespace ABulkCopy.Common.Mapping;

public class ColumnMap
{
    public ColumnMap(
        string sourceType, 
        string targetType, 
        int? sourceLength=null, 
        int? sourcePrecision=null, 
        int? sourceScale=null,
        int? targetLength=null,
        int? targetPrecision=null,
        int? targetScale=null)
    {
        SourceType = sourceType;
        TargetType = targetType;
        SourceLength = sourceLength;
        SourcePrecision = sourcePrecision;
        SourceScale = sourceScale;
        TargetLength = targetLength;
        TargetPrecision = targetPrecision;
        TargetScale = targetScale;
    }

    public string SourceType { get; set; }
    public int? SourceLength { get; set; }
    public int? SourcePrecision { get; set; }
    public int? SourceScale { get; set; }
    public string TargetType { get; set; }
    public int? TargetLength { get; set; }
    public int? TargetPrecision { get; set; }
    public int? TargetScale { get; set; }
}