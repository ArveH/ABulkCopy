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
            await DbFixture.ExecuteNonQuery($"insert into {tableName} (Name) values ('Arve1')");
            await DbFixture.ExecuteNonQuery($"insert into {tableName} (Name) values ('Arve2')");
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

        // Act
        List<decimal> statusValues;
        try
        {
            await DbFixture.PgCmd.CreateTableAsync(inputDefinition, CancellationToken.None);
            await DbFixture.ExecuteNonQuery($"insert into {tableName} (id) values (3)");
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
            st, [
                ("id", true),
                ("col1", false),
                ("col2", false)
            ]);
        await DbFixture.DropTable(st);
        var systemTables = CreatePgSystemTables();
       
        try
        {
            await DbFixture.PgCmd.CreateTableAsync(inputDefinition, CancellationToken.None);

            // Act
            var pk = await systemTables.GetPrimaryKeyAsync(new TableHeader
            {
                Name = st.tableName,
                Schema = st.schemaName
            }, CancellationToken.None);
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
            parent1, [
                ("Parent1Id", true),
                ("col1", false),
                ("col2", false)
            ]);
        await DbFixture.PgCmd.CreateTableAsync(parent1TableDefinition, CancellationToken.None);
        var parent2TableDefinition = GetParentTableDefinition(
            parent2, [
                ("Parent2Id", true),
                ("col1", false),
                ("col2", false)
            ]);
        await DbFixture.PgCmd.CreateTableAsync(parent2TableDefinition, CancellationToken.None);
        var childTableDefinition = GetChildTableDefinition(
            child,
            [
                (parent1, ["Parent1Id"]),
                (parent2, ["Parent2Id"])
            ]);

        try
        {
            // Act
            await DbFixture.PgCmd.CreateTableAsync(childTableDefinition, CancellationToken.None);

            // Assert
            var systemTables = CreatePgSystemTables();
            var fks = (await systemTables.GetForeignKeysAsync(new TableHeader
            {
                Name = child.tableName,
                Schema = child.schemaName
            }, CancellationToken.None)).ToList();
            fks.Count.Should().Be(2, "because there should be 2 foreign keys");
            fks.Select(k => k.TableReference.ToLower()).Should().BeEquivalentTo(
                [parent1.tableName.ToLower(), parent2.tableName.ToLower()],
                "because there should be 2 foreign keys");
            fks.Count(k => k.TableReference.Equals(parent1.tableName, StringComparison.InvariantCultureIgnoreCase))
                .Should().Be(1, $"because there should be a parent: {parent1.tableName}");
            fks.First(k => k.TableReference.Equals(parent1.tableName, StringComparison.InvariantCultureIgnoreCase)).ColumnNames[0]
                .Should().BeEquivalentTo("Parent1Id");
            fks.Count(k => k.TableReference.Equals(parent2.tableName, StringComparison.InvariantCultureIgnoreCase))
                .Should().Be(1, $"because there should be a parent: {parent2.tableName}");
            fks.First(k => k.TableReference.Equals(parent2.tableName, StringComparison.InvariantCultureIgnoreCase)).ColumnNames[0]
                .Should().BeEquivalentTo("Parent2Id");        }
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
        SchemaTableTuple parent = (DatabaseFixture.TestSchemaName, GetName() + "_parent");
        SchemaTableTuple child = ("public", GetName() + "_child");
        await DbFixture.DropTable(child);
        await DbFixture.DropTable(parent);
        var parentDef = GetParentTableDefinition(
            parent, [
                ("ParentId", true),
                ("col1", false)
            ]);
        await DbFixture.PgCmd.CreateTableAsync(parentDef, CancellationToken.None);
        var childTableDefinition = GetChildTableDefinition(
            child,
            [(parent, ["ParentId"])]);

        try
        {
            // Act
            await DbFixture.PgCmd.CreateTableAsync(childTableDefinition, CancellationToken.None);

            // Assert
            var systemTables = CreatePgSystemTables();
            var fks = (await systemTables.GetForeignKeysAsync(new TableHeader
            {
                Name = child.tableName,
                Schema = child.schemaName
            }, CancellationToken.None)).ToList();
            fks.Count.Should().Be(1, "because there be a foreign keys");
            fks[0].SchemaReference.Should().BeEquivalentTo(parent.schemaName);
            fks[0].TableReference.Should().BeEquivalentTo(parent.tableName);
            fks[0].ColumnNames.Count.Should().Be(1);
            fks[0].ColumnNames[0].Should().BeEquivalentTo("ParentId");
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
            parent, [
                ("Parent1Id", true),
                ("col1", true),
                ("col2", false)
            ]);
        await DbFixture.PgCmd.CreateTableAsync(parent1TableDefinition, CancellationToken.None);
        var childTableDefinition = GetChildTableDefinition(
            child,
            [(parent, ["Parent1Id", "col1"])]);

        try
        {
            // Act
            await DbFixture.PgCmd.CreateTableAsync(childTableDefinition, CancellationToken.None);

            // Assert
            var systemTables = CreatePgSystemTables();
            var fks = (await systemTables.GetForeignKeysAsync(new TableHeader
            {
                Name = child.tableName,
                Schema = child.schemaName
            }, CancellationToken.None)).ToList();
            fks.Count.Should().Be(1, "because there should be 1 foreign key");
            fks[0].TableReference.Should().BeEquivalentTo(parent.tableName);
            fks[0].ColumnReferences.Count.Should().Be(2);
            fks[0].ColumnReferences.Should().BeEquivalentTo("parent1id", "col1");
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
            parent, [
                ("Parent1Id", true),
                ("col1", true),
                ("col2", false)
            ]);
        await DbFixture.PgCmd.CreateTableAsync(parent1TableDefinition, CancellationToken.None);
        await DbFixture.PgRawCommand.ExecuteNonQueryAsync($"insert into {parent.tableName} (Parent1Id, col1, col2) values (1, 1, 1)", CancellationToken.None);
        await DbFixture.PgRawCommand.ExecuteNonQueryAsync($"insert into {parent.tableName} (Parent1Id, col1, col2) values (1, 2, 1)", CancellationToken.None);
        var childTableDefinition = GetChildTableDefinition(
            child,
            [(parent, ["Parent1Id", "col1"])]);
        childTableDefinition.ForeignKeys[0].DeleteAction = DeleteAction.Cascade;
        await DbFixture.PgCmd.CreateTableAsync(childTableDefinition, CancellationToken.None);
        await DbFixture.PgRawCommand.ExecuteNonQueryAsync($"insert into {child.tableName} (Id, Parent1Id, col1) values (10, 1, 1)", CancellationToken.None);
        await DbFixture.PgRawCommand.ExecuteNonQueryAsync($"insert into {child.tableName} (Id, Parent1Id, col1) values (11, 1, 2)", CancellationToken.None);
        var beforeCount = (long)(await DbFixture.PgRawCommand.ExecuteScalarAsync($"select count(*) from {child.tableName}", CancellationToken.None) ?? 0);
        beforeCount.Should().Be(2, "because child table has two rows before deleting from parent table");

        try
        {
            // Act
            await DbFixture.PgRawCommand.ExecuteNonQueryAsync($"delete from {parent.tableName} where col1 = 1", CancellationToken.None);

            // Assert
            var afterCount = (long)(await DbFixture.PgRawCommand.ExecuteScalarAsync($"select count(*) from {child.tableName}", CancellationToken.None) ?? 0);
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