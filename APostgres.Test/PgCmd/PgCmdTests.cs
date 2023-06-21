﻿namespace APostgres.Test.PgCmd;

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
            await PgDbHelper.Instance.CreateTable(inputDefinition);
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
            await PgDbHelper.Instance.CreateTable(inputDefinition);
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
}