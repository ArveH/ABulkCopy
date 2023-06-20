namespace APostgres.Test.PgCmd;

public class PgCmdTests : PgTestBase
{
    public PgCmdTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task TestCreateTable_When_IdentityColumn()
    {
        // Arrange
        var tableName = GetName();
        var inputDefinition = MssTestData.GetEmpty(tableName);
        inputDefinition.Header.Identity = new Identity
        {
            Increment = 10,
            Seed = 100
        };
        var identityCol = new SqlServerBigInt(1, "agrtid", false)
        {
            Identity = inputDefinition.Header.Identity
        };
        inputDefinition.Columns.Add(identityCol);
        inputDefinition.Columns.Add(new SqlServerNVarChar(2, "Name", true, 100));
        await PgDbHelper.Instance.DropTable(tableName);

        // Act
        List<long> identityValues;
        try
        {
            await PgDbHelper.Instance.CreateTable(inputDefinition);
            await PgDbHelper.Instance.ExecuteNonQuery($"insert into {tableName} (Name) values ('Arve1')");
            await PgDbHelper.Instance.ExecuteNonQuery($"insert into {tableName} (Name) values ('Arve2')");
            identityValues = (await PgDbHelper.Instance.SelectColumn<long>(tableName, "agrtid")).ToList();
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(tableName);
        }
        
        // Assert
        identityValues.Count.Should().Be(2);
        identityValues[0].Should().Be(100);
        identityValues[1].Should().Be(110);
    }
}