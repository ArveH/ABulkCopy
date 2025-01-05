namespace CrossRDBMS.Tests;

[Collection(nameof(DatabaseCollection))]
public class TestSchemas : TestBase
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _output;

    public TestSchemas(
        DatabaseFixture fixture, 
        ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    [Fact]
    public async Task TestFKInAnotherSchema()
    {
        // Arrange
        var baseName = GetName();
        var parentName = baseName + "_parent";
        var childName = baseName + "_child";
        var parentId = 1;
        var childId = 100;

        List<string> logMessages = new();
        IFileSystem fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {
                "mymappings.json", new MockFileData(
                    "{\r\n" +
                    "    \"Schemas\": {\r\n" +
                    "        \"\": \"public\",\r\n" +
                    "        \"dbo\": \"public\",\r\n" +
                    "        \"my_mss_schema\": \"my_pg_schema\"\r\n" +
                    "    },\r\n    \"Collations\": {\r\n" +
                    "        \"SQL_Latin1_General_CP1_CI_AI\": \"en_ci_ai\",\r\n" +
                    "        \"SQL_Latin1_General_CP1_CI_AS\": \"en_ci_as\"\r\n" +
                    "    }\r\n" +
                    "}")
            }
        });

        var mssContext = new CopyContext(
            Rdbms.Mss,
            CmdArguments.Create(ParamHelper.GetOutMss(
                _fixture.MssConnectionString,
                searchFilter: "%" + baseName + "%")),
            logMessages,
            _output,
            fileSystem);
        var pgContext = new CopyContext(
            Rdbms.Pg,
            CmdArguments.Create(ParamHelper.GetInPg(
                _fixture.PgConnectionString,
                mappingsFile: "mymappings.json")),
            logMessages,
            _output,
            fileSystem);

        await _fixture.MssCmd.DropTableAsync(("dbo", childName), CancellationToken.None);
        await _fixture.MssCmd.DropTableAsync((DatabaseFixture.MssTestSchemaName, parentName), CancellationToken.None);
        var parentDef = await CreateTableAsync(
            (DatabaseFixture.MssTestSchemaName, parentName),
            new SqlServerInt(101, "id", false),
            parentId);
        var childDef = await CreateChildTableAsync(
            ("dbo", childName),
            new SqlServerInt(101, "id", false),
            parentDef,
            (parentId, childId));

        var copyOut = mssContext.GetServices<ICopyOut>();
        var copyIn = pgContext.GetServices<ICopyIn>();

        // Act
        await copyOut.RunAsync(CancellationToken.None);
        await copyIn.RunAsync(Rdbms.Pg, CancellationToken.None);

        // Assert
        await AssertFilesExists(
            fileSystem, parentDef.GetSchemaFileName(), parentDef.Data.FileName);
        await AssertFilesExists(
            fileSystem, childDef.GetSchemaFileName(), childDef.Data.FileName);
        await ValidateValueAsync(
            ("public", childDef.Header.Name),
            childDef.Columns[0].Name,
            childId);
        await ValidateValueAsync(
            ("public", childDef.Header.Name),
            childDef.Columns[1].Name,
            parentId);
        await ValidateValueAsync(
            (DatabaseFixture.PgTestSchemaName, parentDef.Header.Name),
            parentDef.Columns[0].Name,
            parentId);
    }

    private static async Task AssertFilesExists(
        IFileSystem fileSystem, string schemaFile, string dataFile)
    {
        var schemaFileContent = await fileSystem.File.ReadAllTextAsync(schemaFile);
        schemaFileContent.Should().NotBeNullOrEmpty("because the schema file should exist");
        schemaFileContent.Should().Contain("\"Name\": \"");
        var dataFileContent = await fileSystem.File.ReadAllTextAsync(dataFile);
        dataFileContent.Should().NotBeNull("because the data file should exist");
    }

    private async Task ValidateValueAsync(
        SchemaTableTuple st, string colName, int expected)
    {
        await using var cmd = _fixture.PgContext.DataSource.CreateCommand(
            $"select {colName} from {st.GetFullName()}");
        var actual = (int?)await cmd.ExecuteScalarAsync();
        actual.Should().Be(expected);
    }

    private async Task<TableDefinition> CreateTableAsync(
        SchemaTableTuple st,
        IColumn mssCol,
        int value)
    {
        var tableDef = MssTestData.GetEmpty(st);
        tableDef.Columns.Add(mssCol);
        tableDef.PrimaryKey = new PrimaryKey
        {
            ColumnNames = [new() { Name = mssCol.Name }]
        };
        await _fixture.MssCmd.CreateTableAsync(tableDef, CancellationToken.None);
        await _fixture.MssRawCommand.ExecuteNonQueryAsync(
            $"insert into {st.GetFullName()}({mssCol.Name}) values ({value})",
            CancellationToken.None);
        return tableDef;
    }

    private async Task<TableDefinition> CreateChildTableAsync(
        SchemaTableTuple st,
        IColumn mssCol,
        TableDefinition parent,
        (int parentId, int childId) values)
    {
        var tableDef = MssTestData.GetEmpty(st);
        tableDef.Columns.Add(mssCol);
        tableDef.Columns.Add(new SqlServerInt(102, "parentid", false));
        tableDef.PrimaryKey = new PrimaryKey
        {
            ColumnNames = [new() { Name = mssCol.Name }]
        };
        tableDef.ForeignKeys.Add(new ForeignKey
        {
            ColumnNames = ["parentid"],
            SchemaReference = parent.Header.Schema,
            TableReference = parent.Header.Name,
            ColumnReferences = [parent.Columns.First().Name],
            DeleteAction = DeleteAction.Cascade
        });
        await _fixture.MssCmd.CreateTableAsync(tableDef, CancellationToken.None);
        await _fixture.MssRawCommand.ExecuteNonQueryAsync(
            $"insert into {st.GetFullName()}({mssCol.Name}, parentid) values ({values.childId}, {values.parentId})",
            CancellationToken.None);

        return tableDef;
    }
}