namespace APostgres.Test.PgCmd;

public class PgCmdTests : PgTestBase
{
    private readonly IPgCmd _pgCmd;

    public PgCmdTests(ITestOutputHelper output) : base(output)
    {
        _pgCmd = new ABulkCopy.APostgres.PgCmd(PgContext, TestLogger);
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
    public async Task TestCreateTable_When_VarCharDefault()
    {
        // Arrange
        var tableName = GetName();
        var inputDefinition = PgTestData.GetEmpty(tableName);
        var defCol = new PostgresVarChar(2, "status", false, 50)
        {
            DefaultConstraint = new DefaultDefinition
            {
                Name = "DF__atswbstas__activ__1DE5A643",
                Definition = "(N'Inactive')",
                IsSystemNamed = true
            }
        };
        inputDefinition.Columns.Add(new PostgresBigInt(1, "id", false));
        inputDefinition.Columns.Add(defCol);
        await PgDbHelper.Instance.DropTable(tableName);

        // Act
        List<string?> statusValues;
        try
        {
            await _pgCmd.CreateTable(inputDefinition);
            await PgDbHelper.Instance.ExecuteNonQuery($"insert into \"{tableName}\" (id) values (3)");
            statusValues = (await PgDbHelper.Instance.SelectColumn<string>(tableName, "status")).ToList();
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(tableName);
        }

        // Assert
        statusValues.Count.Should().Be(1);
        statusValues[0].Should().Be("Inactive");
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

        // Act
        List<decimal> statusValues;
        try
        {
            await _pgCmd.CreateTable(inputDefinition);
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
    public async Task TestCreateTable_When_UuidDefault()
    {
        // Arrange
        var tableName = GetName();
        var inputDefinition = PgTestData.GetEmpty(tableName);
        var defCol = new PostgresUuid(2, "status", false)
        {
            DefaultConstraint = new DefaultDefinition
            {
                Name = "DF__atswbstas__activ__1DE5A643",
                Definition = "newid()",
                IsSystemNamed = true
            }
        };
        inputDefinition.Columns.Add(new PostgresBigInt(1, "id", false));
        inputDefinition.Columns.Add(defCol);
        await PgDbHelper.Instance.DropTable(tableName);

        // Act
        List<Guid> statusValues;
        try
        {
            await _pgCmd.CreateTable(inputDefinition);
            await PgDbHelper.Instance.ExecuteNonQuery($"insert into \"{tableName}\" (id) values (3)");
            statusValues = (await PgDbHelper.Instance.SelectColumn<Guid>(tableName, "status")).ToList();
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(tableName);
        }

        // Assert
        statusValues.Count.Should().Be(1);
        statusValues[0].Should().NotBe(Guid.Empty);
    }

    [Theory]
    [InlineData("(CONVERT([datetime],N'19000101 00:00:00:000',(9)))", "19000101 00:00:00:000")]
    [InlineData("(CONVERT([datetime],'JAN 1 1900 00:00:01:000',(9)))", "19000101 00:00:01:000")]
    [InlineData("(CONVERT([datetime],'20991231 23:59:59:998',(9)))", "20991231 23:59:59:998")]
    [InlineData("(CONVERT([datetime],'JAN 1 1900',(9)))", "19000101 00:00:00:000")]
    [InlineData("(getdate())", "today")]
    public async Task TestCreateTable_When_MssDateTimeDefault(string val, string expected)
    {
        // Arrange
        var tableName = GetName();
        var inputDefinition = PgTestData.GetEmpty(tableName);
        var defCol = new PostgresTimestamp(2, "status", false)
        {
            DefaultConstraint = new DefaultDefinition
            {
                Name = "DF__atswbstas__activ__1DE5A643",
                Definition = val,
                IsSystemNamed = true
            }
        };

        inputDefinition.Columns.Add(new PostgresBigInt(1, "id", false));
        inputDefinition.Columns.Add(defCol);
        await PgDbHelper.Instance.DropTable(tableName);

        // Act
        List<DateTime> statusValues;
        try
        {
            await _pgCmd.CreateTable(inputDefinition);
            await PgDbHelper.Instance.ExecuteNonQuery($"insert into \"{tableName}\" (id) values (3)");
            statusValues = (await PgDbHelper.Instance.SelectColumn<DateTime>(tableName, "status")).ToList();
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(tableName);
        }

        // Assert
        statusValues.Count.Should().Be(1);
        var actual = statusValues[0].ToString("yyyyMMdd HH:mm:ss:fff");
        if (expected == "today") expected = DateTime.Today.ToString("yyyyMMdd HH:mm:ss:fff");
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("((0))", false)]
    [InlineData("((1))", true)]
    public async Task TestCreateTable_When_MssBitDefault(string val, bool expected)
    {
        // Arrange
        var tableName = GetName();
        var inputDefinition = PgTestData.GetEmpty(tableName);
        var defCol = new PostgresBoolean(2, "status", false)
        {
            DefaultConstraint = new DefaultDefinition
            {
                Name = "DF__atswbstas__activ__1DE5A643",
                Definition = val,
                IsSystemNamed = true
            }
        };

        inputDefinition.Columns.Add(new PostgresBigInt(1, "id", false));
        inputDefinition.Columns.Add(defCol);
        await PgDbHelper.Instance.DropTable(tableName);

        // Act
        List<bool> statusValues;
        try
        {
            await _pgCmd.CreateTable(inputDefinition);
            await PgDbHelper.Instance.ExecuteNonQuery($"insert into \"{tableName}\" (id) values (3)");
            statusValues = (await PgDbHelper.Instance.SelectColumn<bool>(tableName, "status")).ToList();
        }
        finally
        {
            await PgDbHelper.Instance.DropTable(tableName);
        }

        // Assert
        statusValues.Count.Should().Be(1);
        statusValues[0].Should().Be(expected);
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

        try
        {
            // Act
            await _pgCmd.CreateTable(inputDefinition);

            // Assert
            IPgSystemTables systemTables = new PgSystemTables(PgContext, TestLogger);
            var pk = await systemTables.GetPrimaryKey(new TableHeader
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
    public async Task TestCreateTable_When_ForeignKey()
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
        await _pgCmd.CreateTable(parent1TableDefinition);
        var parent2TableDefinition = GetParentTableDefinition(
            parent2TableName, new List<(string, bool)>
            {
                ("Parent2Id", true),
                ("col1", false),
                ("col2", false),
            });
        await _pgCmd.CreateTable(parent2TableDefinition);
        var childTableDefinition = GetChildTableDefinition(
            childTableName,
            new List<(string, string)>
            {
                (parent1TableName, "Parent1Id"),
                (parent2TableName, "Parent2Id")
            });

        try
        {
            // Act
            await _pgCmd.CreateTable(childTableDefinition);

            // Assert
            IPgSystemTables systemTables = new PgSystemTables(PgContext, TestLogger);
            var fks = (await systemTables.GetForeignKeys(new TableHeader
            {
                Name = childTableName,
                Schema = "public"
            })).ToList();
            fks.Count.Should().Be(2, "because there should be 2 foreign keys");
            fks.Select(k => k.TableReference).Should().Contain(new List<string>{parent1TableName, parent2TableName});
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
        List<(string tabName, string colName)> refs)
    {
        var inputDefinition = PgTestData.GetEmpty(tableName);
        inputDefinition.Columns.Add(new PostgresBigInt(0, "id", false));
        inputDefinition.PrimaryKey = new PrimaryKey
        {
            Name = $"PK_{tableName}_id",
            ColumnNames = new List<OrderColumn> { new() { Name = "id" } }
        };
        for (var i = 0; i < refs.Count; i++)
        {
            inputDefinition.Columns.Add(new PostgresInt(i + 1, refs[i].colName, false));
            inputDefinition.ForeignKeys.Add(new ForeignKey
            {
                ConstraintName = $"FK_{tableName}_{refs[i].tabName}_{refs[i].colName}",
                ColumnNames = new List<string> { refs[i].colName },
                TableReference = refs[i].tabName,
                ColumnReferences = new List<string> { refs[i].colName },
            });
        }
        return inputDefinition;
    }
}