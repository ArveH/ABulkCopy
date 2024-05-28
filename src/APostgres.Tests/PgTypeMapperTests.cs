namespace APostgres.Tests;

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
        var defCol = new SqlServerBit(2, "status", false)
        {
            DefaultConstraint = new DefaultDefinition
            {
                Name = "DF__atswbstas__activ__1DE5A643",
                Definition = val,
                IsSystemNamed = true
            }
        };

        TestConvert(GetName(), defCol, "smallint", expected);
    }

    [Theory]
    [InlineData("(CONVERT([datetime],N'19000101 00:00:00:000',(9)))", "(to_timestamp('19000101 00:00:00:000', 'YYYYMMDD HH24:MI:SS:FF3'))")]
    [InlineData("(CONVERT([datetime],'JAN 1 1900 00:00:01:000',(9)))", "(cast('JAN 1 1900 00:00:01' as timestamp))")]
    [InlineData("(CONVERT([datetime],'20991231 23:59:59:998',(9)))", "(to_timestamp('20991231 23:59:59:998', 'YYYYMMDD HH24:MI:SS:FF3'))")]
    [InlineData("(CONVERT([datetime],'JAN 1 1900',(9)))", "(cast('JAN 1 1900' as timestamp))")]
    [InlineData("(getdate())", "(localtimestamp)")]
    public void TestConvert_When_MssDateTimeDefault(string val, string expected)
    {
        var defCol = new SqlServerDateTime2(2, "LastUpdate", false)
        {
            DefaultConstraint = new DefaultDefinition
            {
                Name = "DF__atswbstas__activ__1DE5A643",
                Definition = val,
                IsSystemNamed = true
            }
        };

        TestConvert(GetName(), defCol, "timestamp", expected);
    }

    [Theory]
    [InlineData("(newid())", "(gen_random_uuid())")]
    public void TestConvert_When_MssGuidDefault(string val, string expected)
    {
        var defCol = new SqlServerUniqueIdentifier(2, "Guid", false)
        {
            DefaultConstraint = new DefaultDefinition
            {
                Name = "DF__atswbstas__activ__1DE5A643",
                Definition = val,
                IsSystemNamed = true
            }
        };

        TestConvert(GetName(), defCol, "uuid", expected);
    }

    private void TestConvert(string tableName, IColumn defCol, string expectedType, string expectedDefault)
    {
        // Arrange
        var inputDefinition = MssTestData.GetEmpty(("dbo", tableName));

        inputDefinition.Columns.Add(new SqlServerBigInt(1, "id", false));
        inputDefinition.Columns.Add(defCol);
        var typeConverter = new PgTypeMapper(
            new PgParser(),
            new ParseTree(new NodeFactory(), new SqlTypes()),
            new TokenizerFactory(new TokenFactory()),
            new PgColumnFactory(), 
            new MappingFactory(
                TestConfiguration,
                new MockFileSystem(),
                TestLogger));

        // Act
        var tableDefinition = typeConverter.Convert(inputDefinition);

        // Assert
        tableDefinition.Columns[1].Type.Should().Be(expectedType);
        tableDefinition.Columns[1].HasDefault.Should().BeTrue();
        tableDefinition.Columns[1].DefaultConstraint!.Definition.Should().Be(expectedDefault);
    }
}