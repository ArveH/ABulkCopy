namespace APostgres.Test;

public class PgTypeMapperTests : PgTestBase
{
    public PgTypeMapperTests(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData("((0))", "((0))")]
    [InlineData("((1))", "((1))")]
    public void TestConvert_When_MssBitDefault(string val, string expected)
    {
        // Arrange
        var tableName = GetName();
        var inputDefinition = MssTestData.GetEmpty(tableName);
        var defCol = new SqlServerBit(2, "status", false)
        {
            DefaultConstraint = new DefaultDefinition
            {
                Name = "DF__atswbstas__activ__1DE5A643",
                Definition = val,
                IsSystemNamed = true
            }
        };
        inputDefinition.Columns.Add(defCol);
        var typeConverter = new PgTypeMapper(
            new PgParser(), new PgColumnFactory(), new MappingFactory());

        // Act
        var tableDefinition = typeConverter.Convert(inputDefinition);

        // Assert
        tableDefinition.Columns[0].Type.Should().Be("smallint");
        tableDefinition.Columns[0].HasDefault.Should().BeTrue();
        tableDefinition.Columns[0].DefaultConstraint!.Definition.Should().Be(expected);
    }

    [Theory]
    [InlineData("(CONVERT([datetime],N'19000101 00:00:00:000',(9)))", "(to_timestamp('19000101 00:00:00:000', 'YYYYMMDD HH24:MI:SS:FF3'))")]
    [InlineData("(CONVERT([datetime],'JAN 1 1900 00:00:01:000',(9)))", "(cast('JAN 1 1900 00:00:01' as timestamp))")]
    [InlineData("(CONVERT([datetime],'20991231 23:59:59:998',(9)))", "(to_timestamp('20991231 23:59:59:998', 'YYYYMMDD HH24:MI:SS:FF3'))")]
    [InlineData("(CONVERT([datetime],'JAN 1 1900',(9)))", "(cast('JAN 1 1900' as timestamp))")]
    [InlineData("(getdate())", "(localtimestamp)")]
    public void TestConvert_When_MssDateTimeDefault(string val, string expected)
    {
        // Arrange
        var tableName = GetName();
        var inputDefinition = MssTestData.GetEmpty(tableName);
        var defCol = new SqlServerDateTime2(2, "LastUpdate", false)
        {
            DefaultConstraint = new DefaultDefinition
            {
                Name = "DF__atswbstas__activ__1DE5A643",
                Definition = val,
                IsSystemNamed = true
            }
        };

        inputDefinition.Columns.Add(new SqlServerBigInt(1, "id", false));
        inputDefinition.Columns.Add(defCol);
        var typeConverter = new PgTypeMapper(
            new PgParser(), new PgColumnFactory(), new MappingFactory());

        // Act
        var tableDefinition = typeConverter.Convert(inputDefinition);

        // Assert
        tableDefinition.Columns[1].Type.Should().Be("timestamp");
        tableDefinition.Columns[1].HasDefault.Should().BeTrue();
        tableDefinition.Columns[1].DefaultConstraint!.Definition.Should().Be(expected);
    }

    [Theory]
    [InlineData("(newid())", "(gen_random_uuid())")]
    public void TestConvert_When_MssGuidDefault(string val, string expected)
    {
        // Arrange
        var tableName = GetName();
        var inputDefinition = MssTestData.GetEmpty(tableName);
        var defCol = new SqlServerUniqueIdentifier(2, "Guid", false)
        {
            DefaultConstraint = new DefaultDefinition
            {
                Name = "DF__atswbstas__activ__1DE5A643",
                Definition = val,
                IsSystemNamed = true
            }
        };

        inputDefinition.Columns.Add(new SqlServerBigInt(1, "id", false));
        inputDefinition.Columns.Add(defCol);
        var typeConverter = new PgTypeMapper(
            new PgParser(), new PgColumnFactory(), new MappingFactory());

        // Act
        var tableDefinition = typeConverter.Convert(inputDefinition);

        // Assert
        tableDefinition.Columns[1].Type.Should().Be("uuid");
        tableDefinition.Columns[1].HasDefault.Should().BeTrue();
        tableDefinition.Columns[1].DefaultConstraint!.Definition.Should().Be(expected);
    }
}