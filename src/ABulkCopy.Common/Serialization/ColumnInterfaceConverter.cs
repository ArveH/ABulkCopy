namespace ABulkCopy.Common.Serialization;

public class ColumnInterfaceConverter : JsonConverter<IColumn>
{
    public override IColumn? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        JsonSerializer.Deserialize<DefaultColumn>(ref reader, options);

    public override void Write(Utf8JsonWriter writer, IColumn value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, (DefaultColumn)value, options);
}