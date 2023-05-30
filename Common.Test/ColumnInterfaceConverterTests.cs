namespace Common.Test;

public class ColumnInterfaceConverterTests
{
    [Fact]
    public void TestDeserializeIColumn()
    {
        // Arrange
        var jsonTxt = JsonSerializer.Serialize(
            new DefaultColumn(1, "MyCol", false) {Type = ColumnType.BigInt});

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(), new ColumnInterfaceConverter() }
        };  
        var column = JsonSerializer.Deserialize<IColumn>(jsonTxt, options);

        // Assert
        column.Should().NotBeNull();
        column!.Id.Should().Be(1);
        column.Name.Should().Be("MyCol");
        column.Type.Should().Be(ColumnType.BigInt);
        column.IsNullable.Should().BeFalse();
    }

    [Fact]
    public void TestSerializeTableDefinition()
    {
        // Arrange
        var tableDefinition = MssTestData.GetTableDefinitionAllTypes();

        // Act
        var options = new JsonSerializerOptions
        {
            // Contrast is going to have a field day with me allowing stuff like ' :-)
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter(), new ColumnInterfaceConverter() }
        };  
        var jsonText = JsonSerializer.Serialize(tableDefinition, options);

        // Assert
        jsonText.Should().StartWith("{\r\n  \"Header\": {\r\n    \"Id\": 1,\r\n    \"Name\": \"MssAllTypes\",\r\n    \"Schema\": \"dbo\",\r\n    \"Location\": \"default\",\r\n    \"Identity\": {\r\n      \"Seed\": 1,\r\n      \"Increment\": 1\r\n    }\r\n  },\r\n  \"Columns\": [\r\n    {\r\n      \"Id\": 101,\r\n      \"Name\": \"Id\",\r\n      \"Type\": \"BigInt\",\r\n      \"IsComputed\": false,\r\n      \"ComputedDefinition\": null,\r\n      \"IsNullable\": false,\r\n      \"Identity\": {\r\n        \"Seed\": 1,\r\n        \"Increment\": 1");
    }

    [Fact]
    public void TestDeSerializeTableDefinition()
    {
        // Arrange
        var tableDefinition = MssTestData.GetTableDefinitionAllTypes();
        var options = new JsonSerializerOptions
        {
            // Contrast is going to have a field day with me allowing stuff like ' :-)
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter(), new ColumnInterfaceConverter() }
        };  
        var jsonText = JsonSerializer.Serialize(tableDefinition, options);

        
        // Act
        var res = JsonSerializer.Deserialize<TableDefinition>(jsonText, options);

        // Assert
        res.Should().BeEquivalentTo(tableDefinition);
    }
}