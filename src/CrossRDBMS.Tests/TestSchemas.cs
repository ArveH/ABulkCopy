using ABulkCopy.Common.Types.Table;

namespace CrossRDBMS.Tests;

[Collection(nameof(DatabaseCollection))]
public class TestSchemas : TestBase
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _output;

    public TestSchemas(DatabaseFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    [Fact]
    public async Task TestFKInAnotherSchema()
    {
        // Arrange
        var baseName = GetName(nameof(TestFKInAnotherSchema));
        var parentName = baseName + "_parent";
        var childName = baseName + "_child";
        var parentId = 1;
        var childId = 100;

        List<string> logMessages = new();
        IFileSystem fileSystem = new MockFileSystem();

        var mssContext = new CopyContext(
            Rdbms.Mss,
            CmdArguments.Create(ParamHelper.GetOutMss(
                _fixture.MssConnectionString,
                searchFilter: baseName + "%")),
            logMessages,
            _output,
            fileSystem);
        var pgContext = new CopyContext(
            Rdbms.Pg,
            CmdArguments.Create(ParamHelper.GetInPg(
                _fixture.PgConnectionString)),
            logMessages,
            _output,
            fileSystem);

        var parentDef = await CreateTableAsync(
            ("dbo", parentName),
            new SqlServerInt(101, "id", false),
            parentId);
        await CreateChildTableAsync(
            ("dbo", childName),
            new SqlServerInt(101, "id", false),
            parentDef,
            (parentId, childId));

        var copyOut = mssContext.GetServices<ICopyOut>();
        var copyIn = pgContext.GetServices<ICopyIn>();
    }

    private async Task<TableDefinition> CreateTableAsync(
        SchemaTableTuple st,
        IColumn mssCol,
        int value)
    {
        await _fixture.MssDbHelper.DropTableAsync(st);
        var tableDef = MssTestData.GetEmpty(st);
        tableDef.Columns.Add(mssCol);
        await _fixture.MssDbHelper.CreateTableAsync(tableDef);
        await _fixture.MssDbHelper.ExecuteNonQueryAsync(
            $"insert into {st.GetFullName()}({mssCol.Name}) values ({value})",
            CancellationToken.None);
        return tableDef;
    }

    private async Task CreateChildTableAsync(
        SchemaTableTuple st,
        IColumn mssCol,
        TableDefinition parent,
        (int parentId, int childId) values)
    {
        await _fixture.MssDbHelper.DropTableAsync(st);
        var tableDef = MssTestData.GetEmpty(st);
        tableDef.Columns.Add(mssCol);
        tableDef.Columns.Add(new SqlServerInt(102, "parentid", false));
        tableDef.ForeignKeys.Add(new ForeignKey
        {
            ColumnNames = ["parentid"],
            TableReference = parent.Header.Name,
            ColumnReferences = [parent.Columns.First().Name],
            DeleteAction = DeleteAction.Cascade
        });
        await _fixture.MssDbHelper.CreateTableAsync(tableDef);
        await _fixture.MssDbHelper.ExecuteNonQueryAsync(
            $"insert into {st.GetFullName()}({mssCol.Name}, parentid) values ({values.childId}, {values.parentId})",
            CancellationToken.None);
    }
}