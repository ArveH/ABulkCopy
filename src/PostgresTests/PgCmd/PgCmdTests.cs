namespace PostgresTests.PgCmd;

[Collection(nameof(DatabaseCollection))]
public class PgCmdTests : PgTestBase
{
    private readonly Mock<IQueryBuilderFactory> _qbFactoryMock = new();
    private readonly Mock<IPgSystemTables> _systemTablesMock = new();

    public PgCmdTests(DatabaseFixture dbFixture, ITestOutputHelper output) 
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestCreateTable_When_IdentityColumn()
    {
        // Arrange
        var tableName = GetName();
        var inputDefinition = PgTestData.GetEmpty(tableName);
        inputDefinition.Header.Identity = new Identity
        {
            Increment = 10,
            Seed = 100
        };
        var identityCol = new PostgresBigInt(1, "agrtid", false)
        {
            Identity = inputDefinition.Header.Identity
        };
        inputDefinition.Columns.Add(identityCol);
        inputDefinition.Columns.Add(new PostgresVarChar(2, "Name", true, 100));
        await DbFixture.DropTable(tableName);

        // Act
        List<long> identityValues;
        try
        {
            await DbFixture.CreateTable(inputDefinition);
            await DbFixture.ExecuteNonQuery($"insert into \"{tableName}\" (\"Name\") values ('Arve1')");
            await DbFixture.ExecuteNonQuery($"insert into \"{tableName}\" (\"Name\") values ('Arve2')");
            identityValues = (await DbFixture.SelectColumn<long>(tableName, "agrtid")).ToList();
        }
        finally
        {
            await DbFixture.DropTable(tableName);
        }

        // Assert
        identityValues.Count.Should().Be(2);
        identityValues[0].Should().Be(100);
        identityValues[1].Should().Be(110);
    }

    [Fact]
    public async Task TestCreateTable_When_DecimalDefault()
    {
        // Arrange
        var tableName = GetName();
        var inputDefinition = PgTestData.GetEmpty(tableName);
        var defCol = new PostgresDecimal(2, "status", false, 32, 6)
        {
            DefaultConstraint = new DefaultDefinition
            {
                Name = "DF__atswbstas__activ__1DE5A643",
                Definition = "((99.9))",
                IsSystemNamed = true
            }
        };
        inputDefinition.Columns.Add(new PostgresBigInt(1, "id", false));
        inputDefinition.Columns.Add(defCol);
        await DbFixture.DropTable(tableName);
        var pgCmd = GetPgCmd();
        var cts = new CancellationTokenSource();

        // Act
        List<decimal> statusValues;
        try
        {
            await pgCmd.CreateTableAsync(inputDefinition, cts.Token);
            await DbFixture.ExecuteNonQuery($"insert into \"{tableName}\" (id) values (3)");
            statusValues = (await DbFixture.SelectColumn<decimal>(tableName, "status")).ToList();
        }
        finally
        {
            await DbFixture.DropTable(tableName);
        }

        // Assert
        statusValues.Count.Should().Be(1);
        statusValues[0].Should().Be(99.9M);
    }

    [Fact]
    public async Task TestCreateTable_When_PrimaryKey()
    {
        // Arrange
        var tableName = GetName() + "_primary";
        var inputDefinition = GetParentTableDefinition(
            tableName, new List<(string, bool)>
            {
                ("id", true),
                ("col1", false),
                ("col2", false)
            });
        await DbFixture.DropTable(tableName);
        var pgCmd = GetPgCmd();
        var systemTables = GetPgSystemTables();
        var cts = new CancellationTokenSource();

        try
        {
            await pgCmd.CreateTableAsync(inputDefinition, cts.Token);

            // Act
            var pk = await systemTables.GetPrimaryKeyAsync(new TableHeader
            {
                Name = tableName,
                Schema = "public"
            }, cts.Token);
            pk.Should().NotBeNull();
            pk!.ColumnNames.Count.Should().Be(1);
            pk.ColumnNames[0].Name.Should().Be("id");
        }
        finally
        {
            await DbFixture.DropTable(tableName);
        }

    }

    [Fact]
    public async Task TestCreateTable_When_ForeignKeysFromTwoTables()
    {
        // Arrange
        var parent1TableName = GetName() + "_parent1";
        var parent2TableName = GetName() + "_parent2";
        var childTableName = GetName() + "_child";
        await DbFixture.DropTable(childTableName);
        await DbFixture.DropTable(parent1TableName);
        await DbFixture.DropTable(parent2TableName);
        var parent1TableDefinition = GetParentTableDefinition(
            parent1TableName, new List<(string, bool)>
            {
                ("Parent1Id", true),
                ("col1", false),
                ("col2", false),
            });
        var cts = new CancellationTokenSource();
        var pgCmd = GetPgCmd();
        await pgCmd.CreateTableAsync(parent1TableDefinition, cts.Token);
        var parent2TableDefinition = GetParentTableDefinition(
            parent2TableName, new List<(string, bool)>
            {
                ("Parent2Id", true),
                ("col1", false),
                ("col2", false),
            });
        await pgCmd.CreateTableAsync(parent2TableDefinition, cts.Token);
        var childTableDefinition = GetChildTableDefinition(
            childTableName,
            new List<(string, List<string>)>
            {
                (parent1TableName, new() { "Parent1Id" }),
                (parent2TableName, new() { "Parent2Id" })
            });

        try
        {
            // Act
            await pgCmd.CreateTableAsync(childTableDefinition, cts.Token);

            // Assert
            var systemTables = GetPgSystemTables();
            var fks = (await systemTables.GetForeignKeysAsync(new TableHeader
            {
                Name = childTableName,
                Schema = "public"
            }, cts.Token)).ToList();
            fks.Count.Should().Be(2, "because there should be 2 foreign keys");
            fks.Select(k => k.TableReference).Should().Contain(new List<string> { parent1TableName, parent2TableName });
            fks.First(k => k.TableReference == parent1TableName).ColumnNames.Count.Should().Be(1);
            fks.First(k => k.TableReference == parent1TableName).ColumnNames.First().Should().Be("Parent1Id");
            fks.First(k => k.TableReference == parent2TableName).ColumnNames.Count.Should().Be(1);
            fks.First(k => k.TableReference == parent2TableName).ColumnNames.First().Should().Be("Parent2Id");
        }
        finally
        {
            await DbFixture.DropTable(childTableName);
            await DbFixture.DropTable(parent2TableName);
            await DbFixture.DropTable(parent1TableName);
        }

    }

    [Fact]
    public async Task TestCreateTable_When_CompositeForeignKey()
    {
        // Arrange
        var parent1TableName = GetName() + "_parent1";
        var childTableName = GetName() + "_child";
        await DbFixture.DropTable(childTableName);
        await DbFixture.DropTable(parent1TableName);
        var parent1TableDefinition = GetParentTableDefinition(
            parent1TableName, new List<(string, bool)>
            {
                ("Parent1Id", true),
                ("col1", true),
                ("col2", false),
            });
        var pgCmd = GetPgCmd();
        var cts = new CancellationTokenSource();
        await pgCmd.CreateTableAsync(parent1TableDefinition, cts.Token);
        var childTableDefinition = GetChildTableDefinition(
            childTableName,
            new List<(string, List<string>)>
            {
                (parent1TableName, new() { "Parent1Id", "col1" })
            });

        try
        {
            // Act
            await pgCmd.CreateTableAsync(childTableDefinition, cts.Token);

            // Assert
            var systemTables = GetPgSystemTables();
            var fks = (await systemTables.GetForeignKeysAsync(new TableHeader
            {
                Name = childTableName,
                Schema = "public"
            }, cts.Token)).ToList();
            fks.Count.Should().Be(1, "because there should be 1 foreign key");
            fks.First().TableReference.Should().Be(parent1TableName);
            fks.First().ColumnReferences.Count.Should().Be(2);
            fks.First().ColumnReferences.Should().Contain(new List<string> { "Parent1Id", "col1" });
        }
        finally
        {
            await DbFixture.DropTable(childTableName);
            await DbFixture.DropTable(parent1TableName);
        }
    }

    [Fact]
    public async Task TestCascadeDelete()
    {
        // Arrange
        var parent1TableName = GetName() + "_parent1";
        var childTableName = GetName() + "_child";
        await DbFixture.DropTable(childTableName);
        await DbFixture.DropTable(parent1TableName);
        var parent1TableDefinition = GetParentTableDefinition(
            parent1TableName, new List<(string, bool)>
            {
                ("Parent1Id", true),
                ("col1", true),
                ("col2", false),
            });
        var pgCmd = GetPgCmd();
        var cts = new CancellationTokenSource();
        await pgCmd.CreateTableAsync(parent1TableDefinition, cts.Token);
        await pgCmd.ExecuteNonQueryAsync($"insert into \"{parent1TableName}\" (\"Parent1Id\", \"col1\", \"col2\") values (1, 1, 1)", cts.Token);
        await pgCmd.ExecuteNonQueryAsync($"insert into \"{parent1TableName}\" (\"Parent1Id\", \"col1\", \"col2\") values (1, 2, 1)", cts.Token);
        var childTableDefinition = GetChildTableDefinition(
            childTableName,
            new List<(string, List<string>)>
            {
                (parent1TableName, new() { "Parent1Id", "col1" })
            });
        childTableDefinition.ForeignKeys.First().DeleteAction = DeleteAction.Cascade;
        await pgCmd.CreateTableAsync(childTableDefinition, cts.Token);
        await pgCmd.ExecuteNonQueryAsync($"insert into \"{childTableName}\" (\"id\", \"Parent1Id\", \"col1\") values (10, 1, 1)", cts.Token);
        await pgCmd.ExecuteNonQueryAsync($"insert into \"{childTableName}\" (\"id\", \"Parent1Id\", \"col1\") values (11, 1, 2)", cts.Token);
        var beforeCount = (long)(await pgCmd.SelectScalarAsync($"select count(*) from \"{childTableName}\"", cts.Token) ?? 0);
        beforeCount.Should().Be(2, "because child table has two rows before deleting from parent table");

        try
        {
            // Act
            await pgCmd.ExecuteNonQueryAsync($"delete from \"{parent1TableName}\" where \"col1\" = 1", cts.Token);

            // Assert
            var afterCount = (long)(await pgCmd.SelectScalarAsync($"select count(*) from \"{childTableName}\"", cts.Token) ?? 0);
            afterCount.Should().Be(1, "because 1 row from child table should be delete when deleting it's foreign key");
        }
        finally
        {
            await DbFixture.DropTable(childTableName);
            await DbFixture.DropTable(parent1TableName);
        }
    }

    private static TableDefinition GetParentTableDefinition(
        string tableName, List<(string colName, bool isKey)> cols)
    {
        var inputDefinition = PgTestData.GetEmpty(tableName);
        var pkCols = new List<string>();
        for (var i = 0; i < cols.Count; i++)
        {
            inputDefinition.Columns.Add(new PostgresInt(i, cols[i].colName, false));
            if (cols[i].isKey)
            {
                pkCols.Add(cols[i].colName);
            }
        }
        inputDefinition.PrimaryKey = new PrimaryKey
        {
            Name = $"PK_{tableName}_{string.Join('_', pkCols)}",
            ColumnNames = cols.Where(c => c.isKey).Select(c => new OrderColumn { Name = c.colName }).ToList()
        };
        return inputDefinition;
    }

    private static TableDefinition GetChildTableDefinition(
        string tableName,
        List<(string tabName, List<string> colNames)> refs)
    {
        // Create TableDefinition and add primary key
        var inputDefinition = PgTestData.GetEmpty(tableName);
        inputDefinition.Columns.Add(new PostgresBigInt(0, "id", false));
        inputDefinition.PrimaryKey = new PrimaryKey
        {
            Name = $"PK_{tableName}_id",
            ColumnNames = new List<OrderColumn> { new() { Name = "id" } }
        };

        // Add columns
        refs.SelectMany(r => r.colNames).ToList().ForEach(colName =>
        {
            inputDefinition.Columns.Add(new PostgresInt(0, colName, false));
        });

        // Add foreign keys
        refs.ForEach(fkInfo =>
        {
            inputDefinition.ForeignKeys.Add(new ForeignKey
            {
                ConstraintName = $"FK_{tableName}_{fkInfo.tabName}_{string.Join('_', fkInfo.colNames)}",
                ColumnNames = fkInfo.colNames,
                TableReference = fkInfo.tabName,
                ColumnReferences = fkInfo.colNames
            });
        });

        return inputDefinition;
    }

    private IPgSystemTables GetPgSystemTables(Dictionary<string, string?>? appSettings = null)
    {
        appSettings ??= new()
        {
            { Constants.Config.AddQuotes, "true" },
        };

        return new PgSystemTables(DbFixture.PgContext, GetIdentifier(appSettings), TestLogger);
    }

    private IPgCmd GetPgCmd(Dictionary<string, string?>? appSettings=null)
    {
        appSettings ??= new()
        {
            { Constants.Config.AddQuotes, "true" },
        };
        appSettings.Add(
            Constants.Config.ConnectionString, 
            TestConfiguration.SafeGet(Constants.Config.ConnectionString));
        _qbFactoryMock
            .Setup(f => f.GetQueryBuilder())
            .Returns(() => new QueryBuilder(
                GetIdentifier(appSettings)));
        return new ABulkCopy.APostgres.PgCmd(
            DbFixture.PgContext,
            _qbFactoryMock.Object,
            _systemTablesMock.Object,
            TestLogger);
    }
}