namespace APostgres.Test.PgCmd;

public class PgCmdTests : PgTestBase
{
    private readonly Mock<IQueryBuilderFactory> _qbFactoryMock = new();
    private readonly Mock<IPgSystemTables> _systemTablesMock = new();

    public PgCmdTests(ITestOutputHelper output) : base(output)
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
        await PgDbHelper.Instance.DropTable(tableName);

        // Act
        List<long> identityValues;
        try
        {
            await PgDbHelper.Instance.CreateTable(inputDefinition);
            await PgDbHelper.Instance.ExecuteNonQuery($"insert into \"{tableName}\" (\"Name\") values ('Arve1')");
            await PgDbHelper.Instance.ExecuteNonQuery($"insert into \"{tableName}\" (\"Name\") values ('Arve2')");
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
        await PgDbHelper.Instance.DropTable(tableName);
        var pgCmd = GetPgCmd();

        // Act
        List<decimal> statusValues;
        try
        {
            await pgCmd.CreateTableAsync(inputDefinition);
            await PgDbHelper.Instance.ExecuteNonQuery($"insert into \"{tableName}\" (id) values (3)");
            statusValues = (await PgDbHelper.Instance.SelectColumn<decimal>(tableName, "status")).ToList();
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(tableName);
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
        await PgDbHelper.Instance.DropTable(tableName);
        var pgCmd = GetPgCmd();
        var systemTables = GetPgSystemTables();

        try
        {
            await pgCmd.CreateTableAsync(inputDefinition);

            // Act
            var pk = await systemTables.GetPrimaryKeyAsync(new TableHeader
            {
                Name = tableName,
                Schema = "public"
            });
            pk.Should().NotBeNull();
            pk!.ColumnNames.Count.Should().Be(1);
            pk.ColumnNames[0].Name.Should().Be("id");
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(tableName);
        }

    }

    [Fact]
    public async Task TestCreateTable_When_ForeignKeysFromTwoTables()
    {
        // Arrange
        var parent1TableName = GetName() + "_parent1";
        var parent2TableName = GetName() + "_parent2";
        var childTableName = GetName() + "_child";
        await PgDbHelper.Instance.DropTable(childTableName);
        await PgDbHelper.Instance.DropTable(parent1TableName);
        await PgDbHelper.Instance.DropTable(parent2TableName);
        var parent1TableDefinition = GetParentTableDefinition(
            parent1TableName, new List<(string, bool)>
            {
                ("Parent1Id", true),
                ("col1", false),
                ("col2", false),
            });
        var pgCmd = GetPgCmd();
        await pgCmd.CreateTableAsync(parent1TableDefinition);
        var parent2TableDefinition = GetParentTableDefinition(
            parent2TableName, new List<(string, bool)>
            {
                ("Parent2Id", true),
                ("col1", false),
                ("col2", false),
            });
        await pgCmd.CreateTableAsync(parent2TableDefinition);
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
            await pgCmd.CreateTableAsync(childTableDefinition);

            // Assert
            var systemTables = GetPgSystemTables();
            var fks = (await systemTables.GetForeignKeysAsync(new TableHeader
            {
                Name = childTableName,
                Schema = "public"
            })).ToList();
            fks.Count.Should().Be(2, "because there should be 2 foreign keys");
            fks.Select(k => k.TableReference).Should().Contain(new List<string> { parent1TableName, parent2TableName });
            fks.First(k => k.TableReference == parent1TableName).ColumnNames.Count.Should().Be(1);
            fks.First(k => k.TableReference == parent1TableName).ColumnNames.First().Should().Be("Parent1Id");
            fks.First(k => k.TableReference == parent2TableName).ColumnNames.Count.Should().Be(1);
            fks.First(k => k.TableReference == parent2TableName).ColumnNames.First().Should().Be("Parent2Id");
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(childTableName);
            await PgDbHelper.Instance.DropTable(parent2TableName);
            await PgDbHelper.Instance.DropTable(parent1TableName);
        }

    }

    [Fact]
    public async Task TestCreateTable_When_CompositeForeignKey()
    {
        // Arrange
        var parent1TableName = GetName() + "_parent1";
        var childTableName = GetName() + "_child";
        await PgDbHelper.Instance.DropTable(childTableName);
        await PgDbHelper.Instance.DropTable(parent1TableName);
        var parent1TableDefinition = GetParentTableDefinition(
            parent1TableName, new List<(string, bool)>
            {
                ("Parent1Id", true),
                ("col1", true),
                ("col2", false),
            });
        var pgCmd = GetPgCmd();
        await pgCmd.CreateTableAsync(parent1TableDefinition);
        var childTableDefinition = GetChildTableDefinition(
            childTableName,
            new List<(string, List<string>)>
            {
                (parent1TableName, new() { "Parent1Id", "col1" })
            });

        try
        {
            // Act
            await pgCmd.CreateTableAsync(childTableDefinition);

            // Assert
            var systemTables = GetPgSystemTables();
            var fks = (await systemTables.GetForeignKeysAsync(new TableHeader
            {
                Name = childTableName,
                Schema = "public"
            })).ToList();
            fks.Count.Should().Be(1, "because there should be 1 foreign key");
            fks.First().TableReference.Should().Be(parent1TableName);
            fks.First().ColumnReferences.Count.Should().Be(2);
            fks.First().ColumnReferences.Should().Contain(new List<string> { "Parent1Id", "col1" });
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(childTableName);
            await PgDbHelper.Instance.DropTable(parent1TableName);
        }
    }

    [Fact]
    public async Task TestCascadeDelete()
    {
        // Arrange
        var parent1TableName = GetName() + "_parent1";
        var childTableName = GetName() + "_child";
        await PgDbHelper.Instance.DropTable(childTableName);
        await PgDbHelper.Instance.DropTable(parent1TableName);
        var parent1TableDefinition = GetParentTableDefinition(
            parent1TableName, new List<(string, bool)>
            {
                ("Parent1Id", true),
                ("col1", true),
                ("col2", false),
            });
        var pgCmd = GetPgCmd();
        await pgCmd.CreateTableAsync(parent1TableDefinition);
        await pgCmd.ExecuteNonQueryAsync($"insert into \"{parent1TableName}\" (\"Parent1Id\", \"col1\", \"col2\") values (1, 1, 1)");
        await pgCmd.ExecuteNonQueryAsync($"insert into \"{parent1TableName}\" (\"Parent1Id\", \"col1\", \"col2\") values (1, 2, 1)");
        var childTableDefinition = GetChildTableDefinition(
            childTableName,
            new List<(string, List<string>)>
            {
                (parent1TableName, new() { "Parent1Id", "col1" })
            });
        childTableDefinition.ForeignKeys.First().DeleteAction = DeleteAction.Cascade;
        await pgCmd.CreateTableAsync(childTableDefinition);
        await pgCmd.ExecuteNonQueryAsync($"insert into \"{childTableName}\" (\"id\", \"Parent1Id\", \"col1\") values (10, 1, 1)");
        await pgCmd.ExecuteNonQueryAsync($"insert into \"{childTableName}\" (\"id\", \"Parent1Id\", \"col1\") values (11, 1, 2)");
        var beforeCount = (long)(await pgCmd.SelectScalarAsync($"select count(*) from \"{childTableName}\"") ?? 0);
        beforeCount.Should().Be(2, "because child table has two rows before deleting from parent table");

        try
        {
            // Act
            await pgCmd.ExecuteNonQueryAsync($"delete from \"{parent1TableName}\" where \"col1\" = 1");

            // Assert
            var afterCount = (long)(await pgCmd.SelectScalarAsync($"select count(*) from \"{childTableName}\"") ?? 0);
            afterCount.Should().Be(1, "because 1 row from child table should be delete when deleting it's foreign key");
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(childTableName);
            await PgDbHelper.Instance.DropTable(parent1TableName);
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

        return new PgSystemTables(PgContext, GetIdentifier(appSettings), TestLogger);
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
            PgContext,
            _qbFactoryMock.Object,
            _systemTablesMock.Object,
            TestLogger);
    }
}