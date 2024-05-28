namespace Postgres.Tests.PgCmd;

[Collection(nameof(DatabaseCollection))]
public class PgCmdTests(
    DatabaseFixture dbFixture, ITestOutputHelper output) 
    : PgTestBase(dbFixture, output)
{
    [Fact]
    public async Task TestCreateTable_When_IdentityColumn()
    {
        // Arrange
        var tableName = GetName();
        var inputDefinition = PgTestData.GetEmpty(("public", tableName));
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
        await DbFixture.DropTable(("public", tableName));

        // Act
        List<long> identityValues;
        try
        {
            await DbFixture.CreateTable(inputDefinition);
            await DbFixture.ExecuteNonQuery($"insert into \"{tableName}\" (\"Name\") values ('Arve1')");
            await DbFixture.ExecuteNonQuery($"insert into \"{tableName}\" (\"Name\") values ('Arve2')");
            identityValues = (await DbFixture.SelectColumn<long>(("public", tableName), "agrtid")).ToList();
        }
        finally
        {
            await DbFixture.DropTable(("public", tableName));
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
        var inputDefinition = PgTestData.GetEmpty(("public", tableName));
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
        await DbFixture.DropTable(("public", tableName));
        var pgCmd = GetPgCmd();
        var cts = new CancellationTokenSource();

        // Act
        List<decimal> statusValues;
        try
        {
            await pgCmd.CreateTableAsync(inputDefinition, cts.Token);
            await DbFixture.ExecuteNonQuery($"insert into \"{tableName}\" (id) values (3)");
            statusValues = (await DbFixture.SelectColumn<decimal>(("public", tableName), "status")).ToList();
        }
        finally
        {
            await DbFixture.DropTable(("public", tableName));
        }

        // Assert
        statusValues.Count.Should().Be(1);
        statusValues[0].Should().Be(99.9M);
    }

    [Fact]
    public async Task TestCreateTable_When_PrimaryKey()
    {
        // Arrange
        SchemaTableTuple st = ("public", GetName() + "_primary");
        var inputDefinition = GetParentTableDefinition(
            st, new List<(string, bool)>
            {
                ("id", true),
                ("col1", false),
                ("col2", false)
            });
        await DbFixture.DropTable(st);
        var pgCmd = GetPgCmd();
        var systemTables = GetPgSystemTables();
        var cts = new CancellationTokenSource();

        try
        {
            await pgCmd.CreateTableAsync(inputDefinition, cts.Token);

            // Act
            var pk = await systemTables.GetPrimaryKeyAsync(new TableHeader
            {
                Name = st.tableName,
                Schema = st.schemaName
            }, cts.Token);
            pk.Should().NotBeNull();
            pk!.ColumnNames.Count.Should().Be(1);
            pk.ColumnNames[0].Name.Should().Be("id");
        }
        finally
        {
            await DbFixture.DropTable(st);
        }

    }

    [Fact]
    public async Task TestCreateTable_When_ForeignKeysFromTwoTables()
    {
        // Arrange
        SchemaTableTuple parent1 = ("public", GetName() + "_parent1");
        SchemaTableTuple parent2 = ("public", GetName() + "_parent2");
        SchemaTableTuple child = ("public", GetName() + "_child");
        await DbFixture.DropTable(child);
        await DbFixture.DropTable(parent1);
        await DbFixture.DropTable(parent2);
        var parent1TableDefinition = GetParentTableDefinition(
            parent1, new List<(string, bool)>
            {
                ("Parent1Id", true),
                ("col1", false),
                ("col2", false),
            });
        var cts = new CancellationTokenSource();
        var pgCmd = GetPgCmd();
        await pgCmd.CreateTableAsync(parent1TableDefinition, cts.Token);
        var parent2TableDefinition = GetParentTableDefinition(
            parent2, new List<(string, bool)>
            {
                ("Parent2Id", true),
                ("col1", false),
                ("col2", false),
            });
        await pgCmd.CreateTableAsync(parent2TableDefinition, cts.Token);
        var childTableDefinition = GetChildTableDefinition(
            child,
            new List<(SchemaTableTuple, List<string>)>
            {
                (parent1, ["Parent1Id"]),
                (parent2, ["Parent2Id"])
            });

        try
        {
            // Act
            await pgCmd.CreateTableAsync(childTableDefinition, cts.Token);

            // Assert
            var systemTables = GetPgSystemTables();
            var fks = (await systemTables.GetForeignKeysAsync(new TableHeader
            {
                Name = child.tableName,
                Schema = child.schemaName
            }, cts.Token)).ToList();
            fks.Count.Should().Be(2, "because there should be 2 foreign keys");
            fks.Select(k => k.TableReference).Should().Contain(
                [parent1.tableName, parent2.tableName]);
            fks.First(k => k.TableReference == parent1.tableName).ColumnNames.Count.Should().Be(1);
            fks.First(k => k.TableReference == parent1.tableName).ColumnNames.First().Should().Be("Parent1Id");
            fks.First(k => k.TableReference == parent2.tableName).ColumnNames.Count.Should().Be(1);
            fks.First(k => k.TableReference == parent2.tableName).ColumnNames.First().Should().Be("Parent2Id");
        }
        finally
        {
            await DbFixture.DropTable(child);
            await DbFixture.DropTable(parent1);
            await DbFixture.DropTable(parent2);
        }

    }

    [Fact]
    public async Task TestCreateTable_When_ForeignKeyFromDifferentSchema()
    {
        // Arrange
        SchemaTableTuple parent = (PgDbHelper.TestSchemaName, GetName() + "_parent");
        SchemaTableTuple child = ("public", GetName() + "_child");
        await DbFixture.DropTable(child);
        await DbFixture.DropTable(parent);
        var parentDef = GetParentTableDefinition(
            parent, new List<(string, bool)>
            {
                ("ParentId", true),
                ("col1", false)
            });
        var pgCmd = GetPgCmd();
        await pgCmd.CreateTableAsync(parentDef, CancellationToken.None);
        var childTableDefinition = GetChildTableDefinition(
            child,
            new List<(SchemaTableTuple, List<string>)>
            {
                (parent, ["ParentId"])
            });

        try
        {
            // Act
            await pgCmd.CreateTableAsync(childTableDefinition, CancellationToken.None);

            // Assert
            var systemTables = GetPgSystemTables();
            var fks = (await systemTables.GetForeignKeysAsync(new TableHeader
            {
                Name = child.tableName,
                Schema = child.schemaName
            }, CancellationToken.None)).ToList();
            fks.Count.Should().Be(1, "because there be a foreign keys");
            fks.First().SchemaReference.Should().Be(parent.schemaName);
            fks.First().TableReference.Should().Be(parent.tableName);
            fks.First().ColumnNames.Count.Should().Be(1);
            fks.First().ColumnNames.First().Should().Be("ParentId");
        }
        finally
        {
            await DbFixture.DropTable(child);
            await DbFixture.DropTable(parent);
        }

    }

    [Fact]
    public async Task TestCreateTable_When_CompositeForeignKey()
    {
        // Arrange
        SchemaTableTuple parent = ("public", GetName() + "_parent");
        SchemaTableTuple child = ("public", GetName() + "_child");
        await DbFixture.DropTable(child);
        await DbFixture.DropTable(parent);
        var parent1TableDefinition = GetParentTableDefinition(
            parent, new List<(string, bool)>
            {
                ("Parent1Id", true),
                ("col1", true),
                ("col2", false),
            });
        var pgCmd = GetPgCmd();
        var cts = new CancellationTokenSource();
        await pgCmd.CreateTableAsync(parent1TableDefinition, cts.Token);
        var childTableDefinition = GetChildTableDefinition(
            child,
            new List<(SchemaTableTuple, List<string>)>
            {
                (parent, ["Parent1Id", "col1"])
            });

        try
        {
            // Act
            await pgCmd.CreateTableAsync(childTableDefinition, cts.Token);

            // Assert
            var systemTables = GetPgSystemTables();
            var fks = (await systemTables.GetForeignKeysAsync(new TableHeader
            {
                Name = child.tableName,
                Schema = child.schemaName
            }, cts.Token)).ToList();
            fks.Count.Should().Be(1, "because there should be 1 foreign key");
            fks.First().TableReference.Should().Be(parent.tableName);
            fks.First().ColumnReferences.Count.Should().Be(2);
            fks.First().ColumnReferences.Should().Contain(new List<string> { "Parent1Id", "col1" });
        }
        finally
        {
            await DbFixture.DropTable(child);
            await DbFixture.DropTable(parent);
        }
    }

    [Fact]
    public async Task TestCascadeDelete()
    {
        // Arrange
        // Arrange
        SchemaTableTuple parent = ("public", GetName() + "_parent");
        SchemaTableTuple child = ("public", GetName() + "_child");
        await DbFixture.DropTable(child);
        await DbFixture.DropTable(parent);
        var parent1TableDefinition = GetParentTableDefinition(
            parent, new List<(string, bool)>
            {
                ("Parent1Id", true),
                ("col1", true),
                ("col2", false),
            });
        var pgCmd = GetPgCmd();
        var cts = new CancellationTokenSource();
        await pgCmd.CreateTableAsync(parent1TableDefinition, cts.Token);
        await pgCmd.ExecuteNonQueryAsync($"insert into \"{parent.tableName}\" (\"Parent1Id\", \"col1\", \"col2\") values (1, 1, 1)", cts.Token);
        await pgCmd.ExecuteNonQueryAsync($"insert into \"{parent.tableName}\" (\"Parent1Id\", \"col1\", \"col2\") values (1, 2, 1)", cts.Token);
        var childTableDefinition = GetChildTableDefinition(
            child,
            new List<(SchemaTableTuple, List<string>)>
            {
                (parent, new() { "Parent1Id", "col1" })
            });
        childTableDefinition.ForeignKeys.First().DeleteAction = DeleteAction.Cascade;
        await pgCmd.CreateTableAsync(childTableDefinition, cts.Token);
        await pgCmd.ExecuteNonQueryAsync($"insert into \"{child.tableName}\" (\"Id\", \"Parent1Id\", \"col1\") values (10, 1, 1)", cts.Token);
        await pgCmd.ExecuteNonQueryAsync($"insert into \"{child.tableName}\" (\"Id\", \"Parent1Id\", \"col1\") values (11, 1, 2)", cts.Token);
        var beforeCount = (long)(await pgCmd.ExecuteScalarAsync($"select count(*) from \"{child.tableName}\"", cts.Token) ?? 0);
        beforeCount.Should().Be(2, "because child table has two rows before deleting from parent table");

        try
        {
            // Act
            await pgCmd.ExecuteNonQueryAsync($"delete from \"{parent.tableName}\" where \"col1\" = 1", cts.Token);

            // Assert
            var afterCount = (long)(await pgCmd.ExecuteScalarAsync($"select count(*) from \"{child.tableName}\"", cts.Token) ?? 0);
            afterCount.Should().Be(1, "because 1 row from child table should be delete when deleting it's foreign key");
        }
        finally
        {
            await DbFixture.DropTable(child);
            await DbFixture.DropTable(parent);
        }
    }

    private static TableDefinition GetParentTableDefinition(
        SchemaTableTuple st, List<(string colName, bool isPrimaryKey)> cols)
    {
        var inputDefinition = PgTestData.GetEmpty(st);
        var pkCols = new List<string>();
        for (var i = 0; i < cols.Count; i++)
        {
            inputDefinition.Columns.Add(new PostgresInt(i, cols[i].colName, false));
            if (cols[i].isPrimaryKey)
            {
                pkCols.Add(cols[i].colName);
            }
        }
        inputDefinition.PrimaryKey = new PrimaryKey
        {
            Name = $"PK_{st.tableName}_{string.Join('_', pkCols)}",
            ColumnNames = cols.Where(c => c.isPrimaryKey).Select(c => new OrderColumn { Name = c.colName }).ToList()
        };
        return inputDefinition;
    }

    private static TableDefinition GetChildTableDefinition(
        SchemaTableTuple st,
        List<(SchemaTableTuple st, List<string> colNames)> refs)
    {
        // Create TableDefinition and add primary key
        var inputDefinition = PgTestData.GetEmpty(st);
        inputDefinition.Columns.Add(new PostgresBigInt(0, "Id", false));
        inputDefinition.PrimaryKey = new PrimaryKey
        {
            Name = $"PK_{st.tableName}_id",
            ColumnNames = [new() { Name = "Id" }]
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
                ConstraintName = $"FK_{st.tableName}_{fkInfo.st.tableName}_{string.Join('_', fkInfo.colNames)}",
                ColumnNames = fkInfo.colNames,
                SchemaReference = fkInfo.st.schemaName,
                TableReference = fkInfo.st.tableName,
                ColumnReferences = fkInfo.colNames
            });
        });

        return inputDefinition;
    }
}