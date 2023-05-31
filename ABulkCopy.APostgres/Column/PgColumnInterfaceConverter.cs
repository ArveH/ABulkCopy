using ABulkCopy.APostgres.Column.ColumnTypes;

namespace ABulkCopy.APostgres.Column;

public class PgColumnInterfaceConverter : JsonConverter<IColumn>
{
    private static JsonSerializerOptions _internalOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter()}
    };

    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(IColumn).IsAssignableFrom(typeToConvert);
    }

    public override IColumn? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        if (!jsonDocument.RootElement.TryGetProperty(nameof(IColumn.Type), out var typeProperty))
        {
            throw new JsonException();
        }

        return (PostgresBigInt?)jsonDocument.RootElement.Deserialize(typeof(PostgresBigInt), _internalOptions);
    }

    public override void Write(Utf8JsonWriter writer, IColumn value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, (DefaultColumn)value, options);
}