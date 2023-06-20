using Testing.Shared.SqlServer;

namespace Common.Test;

public class ColumnInterfaceConverterTests
{
    [Fact]
    public void TestDeserializeIColumn()
    {
        // Arrange
        var jsonTxt = JsonSerializer.Serialize(
            new DefaultColumn(1, MssTypes.BigInt, "MyCol", false));

        // Act
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(), new ColumnInterfaceConverter() }
        };  
        var column = JsonSerializer.Deserialize<IColumn>(jsonTxt, options);

        // Assert
        column.Should().NotBeNull();
        column!.Name.Should().Be("MyCol");
        column.Type.Should().Be(MssTypes.BigInt);
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
        jsonText.Should().StartWith("{\r\n  \"Rdbms\": \"Mss\",\r\n  \"Header\": {\r\n    \"Id\": 1,\r\n    \"Name\": \"MssAllTypes\",\r\n    \"Schema\": \"dbo\",\r\n    \"Location\": \"default\",\r\n    \"Identity\": {\r\n      \"Seed\": 1,\r\n      \"Increment\": 1\r\n    }\r\n  },\r\n  \"Columns\": [\r\n    {\r\n      \"Name\": \"Id\",\r\n      \"Type\": \"bigint\",\r\n      \"IsNullable\": false,\r\n      \"Identity\": {\r\n        \"Seed\": 1,\r\n        \"Increment\": 1\r\n      },\r\n      \"ComputedDefinition\": null,\r\n      \"Length\": 8,\r\n      \"Precision\": 19,\r\n      \"Scale\": 0,\r\n      \"DefaultConstraint\": null,\r\n      \"Collation\": null\r\n    },");
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
        res!.Header.Should().BeEquivalentTo(tableDefinition.Header);
        res.Columns.Count.Should().Be(tableDefinition.Columns.Count);
        res.Columns.Select(c => new
        {
            c.Name, c.Type, c.IsNullable, c.Collation, c.Identity, c.DefaultConstraint, c.ComputedDefinition, c.Length, c.Precision, c.Scale
        }).Should().BeEquivalentTo(tableDefinition.Columns.Select(c => new
        {
            c.Name, c.Type, c.IsNullable, c.Collation, c.Identity, c.DefaultConstraint, c.ComputedDefinition, c.Length, c.Precision, c.Scale
        }));
    }
}