namespace Postgres.Tests.DataReader;

[Collection(nameof(DatabaseCollection))]
public class PgDataReaderMiscTests : PgDataReaderTestBase
{
    public PgDataReaderMiscTests(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestRead_When_EmptyDataFile()
    {
        var tableName = "TestRead_When_EmptyDataFile";
        var fileData = new List<string>();
        var cols = new List<IColumn>
        {
            new PostgresUuid(1, "Id", false),
            new PostgresVarChar(2, "Name", true, 100),
            new PostgresTimestamp(3, "LastUpdate", true),
        };
        try
        {
            // Arrange
            await CreateTableAndReadData(
                tableName, cols, fileData);

            // Assert
            var rowcount = await DbFixture.GetRowCount(("public", tableName));
            rowcount.Should().Be(0);
        }
        finally
        {
            await DbFixture.DropTable(("public", tableName));
        }
    }

    [Fact]
    public async Task Test3Cols()
    {
        var tableName = "Test3Cols";
        var fileData = new List<string>
        {
            $"8e98618c-6a78-4fa2-9c0e-b6bb2571af72,{Constants.QuoteChar}Arve{Constants.QuoteChar},2023-06-14T11:12:13.123456,",
        };
        var cols = new List<IColumn>
        {
            new PostgresUuid(1, "Id", false),
            new PostgresVarChar(2, "Name", true, 100),
            new PostgresTimestamp(3, "LastUpdate", true),
        };
        try
        {
            // Arrange
            await CreateTableAndReadData(
                tableName, cols, fileData);

            // Assert
            var guidValues = await DbFixture.SelectColumn<Guid>(
                ("public", tableName), "Id");
            guidValues.Count.Should().Be(1, "because there is 1 guid value");
            guidValues[0].Should().Be("8e98618c-6a78-4fa2-9c0e-b6bb2571af72");
            var nameValues = await DbFixture.SelectColumn<string>(
                ("public", tableName), "Name");
            nameValues.Count.Should().Be(1, "because there is 1 name value");
            nameValues[0].Should().Be("Arve");
            var lastUpdateValues = await DbFixture.SelectColumn<DateTime>(
                ("public", tableName), "LastUpdate");
            lastUpdateValues.Count.Should().Be(1, "because there is 1 last update value");
            lastUpdateValues[0].Should().Be(new DateTime(2023, 6, 14, 11, 12, 13, 123, 456, DateTimeKind.Utc));
        }
        finally
        {
            await DbFixture.DropTable(("public", tableName));
        }
    }

    [Fact]
    public async Task Test3Rows()
    {
        var tableName = "Test3Rows";
        var fileData = new List<string>
        {
            "8e98618c-6a78-4fa2-9c0e-b6bb2571af72,",
            "8e98618c-6a78-4fa2-9c0e-b6bb2571af73,",
            "8e98618c-6a78-4fa2-9c0e-b6bb2571af74,"
        };
        var cols = new List<IColumn>
        {
            new PostgresUuid(1, "Id", false),
        };
        try
        {
            // Arrange
            await CreateTableAndReadData(
                tableName, cols, fileData);

            // Assert
            // Assert
            var guidValues = await DbFixture.SelectColumn<Guid>(
                ("public", tableName), "Id");
            guidValues.Count.Should().Be(3, "because there are 3 guid values");
            guidValues[0].Should().Be("8e98618c-6a78-4fa2-9c0e-b6bb2571af72");
            guidValues[1].Should().Be("8e98618c-6a78-4fa2-9c0e-b6bb2571af73");
            guidValues[2].Should().Be("8e98618c-6a78-4fa2-9c0e-b6bb2571af74");
        }
        finally
        {
            await DbFixture.DropTable(("public", tableName));
        }
    }

    [Fact(Skip = "Error handling not finished")]
    public async Task TestRead_When_EndQuoteMissing()
    {
        var tableName = "TestRead_When_EndQuoteMissing";
        var fileData = new List<string>
        {
            $"8e98618c-6a78-4fa2-9c0e-b6bb2571af72,{Constants.QuoteChar}Arve{Constants.QuoteChar},2023-06-14T11:12:13.123456,",
            "8e98618c-6a78-4fa2-9c0e-b6bb2571af73,'No end quote,2023-06-15T11:12:13,",
            $"8e98618c-6a78-4fa2-9c0e-b6bb2571af74,{Constants.QuoteChar}Per{Constants.QuoteChar},2023-06-15T11:12:13,"
        };
        var cols = new List<IColumn>
        {
            new PostgresUuid(1, "Id", false),
            new PostgresVarChar(2, "Name", true, 100),
            new PostgresTimestamp(3, "LastUpdate", true),
        };
        try
        {
            // Arrange
            await CreateTableAndReadData(
                tableName, cols, fileData);

            // Assert
            var guidValues = await DbFixture.SelectColumn<Guid>(
                ("public", tableName), "Id");
            guidValues.Count.Should().Be(2, "because there are 2 guid values");
            guidValues[0].Should().Be("8e98618c-6a78-4fa2-9c0e-b6bb2571af72");
            guidValues[1].Should().Be("8e98618c-6a78-4fa2-9c0e-b6bb2571af74");
            var nameValues = await DbFixture.SelectColumn<string>(
                ("public", tableName), "Name");
            nameValues.Count.Should().Be(2, "because there are 2 name values");
            nameValues[0].Should().Be("Arve");
            nameValues[1].Should().Be("Per");
            var lastUpdateValues = await DbFixture.SelectColumn<DateTime>(
                ("public", tableName), "LastUpdate");
            lastUpdateValues.Count.Should().Be(2, "because there are 2 last update values");
            lastUpdateValues[0].Should().Be(new DateTime(2023, 6, 14, 11, 12, 13, 123, 456, DateTimeKind.Utc));
            lastUpdateValues[1].Should().Be(new DateTime(2023, 6, 15, 11, 12, 13, DateTimeKind.Utc));
        }
        finally
        {
            await DbFixture.DropTable(("public", tableName));
        }
    }
}