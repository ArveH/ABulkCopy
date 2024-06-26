﻿// ReSharper disable PossiblyMistakenUseOfInterpolatedStringInsert

using Testing.Shared;

namespace SqlServer.Tests;

[Collection(nameof(DatabaseCollection))]
public class MssDataWriterTestsMisc : MssDataWriterTestBase
{
    public MssDataWriterTestsMisc(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output, Environment.MachineName + "MssDataWriterTestsMisc")
    {
    }

    [Fact]
    public async Task TestWriteGuid()
    {
        var value = Guid.NewGuid();

        var jsonTxt = await ArrangeAndAct(
            new SqlServerUniqueIdentifier(101, "MyTestCol", false),
            value);

        // Assert
        jsonTxt.TrimEnd().Should().Be(value + ",");
    }

    [Fact]
    public async Task TestVarBinary_When_OneRow()
    {
        var value = AllTypes.SampleValues.Varbinary10K;
        var col = new SqlServerVarBinary(101, "MyTestCol", false, 10000);
        OriginalTableDefinition.Columns.Add(col);
        await DropTableAsync(TestTableName);
        await CreateTableAsync(OriginalTableDefinition);
        await InsertIntoSingleColumnTableAsync(
            TestTableName, value, SqlDbType.VarBinary);
        var cts = new CancellationTokenSource();

        // Act
        try
        {
            await TestDataWriter.WriteAsync(
                OriginalTableDefinition,
                TestPath,
                cts.Token);
        }
        finally
        {
            await DropTableAsync(TestTableName);
        }

        // Assert
        var dataFile = await MockFileSystem.GetJsonDataText(TestPath, ("dbo", TestTableName));
        dataFile.TrimEnd().Should().Be("i000000000000000.raw,");
        var fullPath = Path.Combine(TestPath, OriginalTableDefinition.GetFullName(), "MyTestCol", $"i{0:D15}.raw");
        MockFileSystem.FileExists(fullPath).Should().BeTrue($"because '{fullPath}' should exist");
        MockFileSystem.FileInfo.New(fullPath).Length.Should().Be(10000);
    }

    [Fact]
    public async Task TestImage_When_OneRow()
    {
        var value = AllTypes.SampleValues.Varbinary10K;
        var col = new SqlServerImage(101, "MyTestCol", false);
        OriginalTableDefinition.Columns.Add(col);
        await DropTableAsync(TestTableName);
        await CreateTableAsync(OriginalTableDefinition);
        await InsertIntoSingleColumnTableAsync(
            TestTableName, value, SqlDbType.Image);
        var cts = new CancellationTokenSource();

        // Act
        try
        {
            await TestDataWriter.WriteAsync(
                OriginalTableDefinition,
                TestPath, 
                cts.Token);
        }
        finally
        {
            await DropTableAsync(TestTableName);
        }

        // Assert
        var dataFile = await MockFileSystem.GetJsonDataText(TestPath, ("dbo", TestTableName));
        dataFile.TrimEnd().Should().Be("i000000000000000.raw,");
        var fullPath = Path.Combine(TestPath, OriginalTableDefinition.GetFullName(), "MyTestCol", $"i{0:D15}.raw");
        MockFileSystem.FileExists(fullPath).Should().BeTrue($"because '{fullPath}' should exist");
        MockFileSystem.FileInfo.New(fullPath).Length.Should().Be(10000);
    }

    [Fact]
    public async Task TestVarBinary_When_3Rows_And_NullValue()
    {
        var col = new SqlServerVarBinary(101, "MyTestCol", true, -1);
        OriginalTableDefinition.Columns.Add(col);
        await DropTableAsync(TestTableName);
        await CreateTableAsync(OriginalTableDefinition);
        await InsertIntoSingleColumnTableAsync(
            TestTableName, AllTypes.SampleValues.Varbinary10K, SqlDbType.VarBinary);
        await InsertIntoSingleColumnTableAsync(
            TestTableName, null, SqlDbType.VarBinary);
        await InsertIntoSingleColumnTableAsync(
            TestTableName, AllTypes.SampleValues.Binary5K, SqlDbType.VarBinary);
        var cts = new CancellationTokenSource();

        // Act
        try
        {
            await TestDataWriter.WriteAsync(
                OriginalTableDefinition,
                TestPath, 
                cts.Token);
        }
        finally
        {
            await DropTableAsync(TestTableName);
        }

        // Assert
        var dataFile = await MockFileSystem.GetJsonDataText(TestPath, ("dbo", TestTableName));
        var dataFileLines = dataFile.Split(
            Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        dataFileLines.Length.Should().Be(3);
        dataFileLines[0].TrimEnd().Should().Be("i000000000000000.raw,");
        dataFileLines[1].TrimEnd().Should().Be(",");
        dataFileLines[2].TrimEnd().Should().Be("i000000000000002.raw,");

        var fullPath = Path.Combine(TestPath, OriginalTableDefinition.GetFullName(), "MyTestCol", $"i{0:D15}.raw");
        MockFileSystem.FileExists(fullPath).Should().BeTrue($"because '{fullPath}' should exist");
        MockFileSystem.FileInfo.New(fullPath).Length.Should().Be(10000);
    }
}