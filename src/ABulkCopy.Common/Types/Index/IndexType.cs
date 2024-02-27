namespace ABulkCopy.Common.Types.Index;

public enum IndexType
{
    Heap = 0,
    Clustered = 1,
    NonClustered = 2,
    Xml = 3,
    Spatial = 4,
    ClusteredColumnStore = 5,
    NonClusteredColumnStore = 6,
    NonClusteredHash = 7
}