﻿namespace Postgres.Tests.DataReader;

[Collection(nameof(DatabaseCollection))]
public class PgDataReaderNumberTests : PgDataReaderTestBase
{
    private const string ColName = "Col1";

    public PgDataReaderNumberTests(DatabaseFixture dbFixture, ITestOutputHelper output)
        : base(dbFixture, output)
    {
    }

    [Fact]
    public async Task TestBigint()
    {
        var col = new PostgresBigInt(1, ColName, false);
        var colValue = await TestDataReader<long>(
            GetName(), col, AllTypes.SampleValues.BigInt + ",");

        colValue.Should().Be(AllTypes.SampleValues.BigInt);
    }

    [Fact]
    public async Task TestBigInt_When_Null()
    {
        // Arrange
        var col = new PostgresBigInt(1, ColName, true);
        var colValue = await TestDataReader<long?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestInt()
    {
        // Arrange
        var col = new PostgresInt(1, ColName, false);
        var colValue = await TestDataReader<int>(
            GetName(), col, AllTypes.SampleValues.Int + ",");

        colValue.Should().Be(AllTypes.SampleValues.Int);
    }

    [Fact]
    public async Task TestInt_When_Null()
    {
        // Arrange
        var col = new PostgresInt(1, ColName, true);
        var colValue = await TestDataReader<int?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestSmallInt()
    {
        // Arrange
        var col = new PostgresSmallInt(1, ColName, false);
        var colValue = await TestDataReader<int>(
            GetName(), col, AllTypes.SampleValues.SmallInt + ",");

        colValue.Should().Be(AllTypes.SampleValues.SmallInt);
    }

    [Fact]
    public async Task TestSmallInt_When_Null()
    {
        // Arrange
        var col = new PostgresSmallInt(1, ColName, true);
        var colValue = await TestDataReader<int?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestBoolean_When_1()
    {
        // Arrange
        var col = new PostgresBoolean(1, ColName, false);
        var colValue = await TestDataReader<bool>(
            GetName(), col, "1,");

        colValue.Should().BeTrue();
    }

    [Fact]
    public async Task TestBoolean_When_0()
    {
        // Arrange
        var col = new PostgresBoolean(1, ColName, false);
        var colValue = await TestDataReader<bool>(
            GetName(), col, "0,");

        colValue.Should().BeFalse();
    }

    [Fact]
    public async Task TestBoolean_When_true()
    {
        // Arrange
        var col = new PostgresBoolean(1, ColName, false);
        var colValue = await TestDataReader<bool>(
            GetName(), col, "true,");

        colValue.Should().BeTrue();
    }

    [Fact]
    public async Task TestBoolean_When_false()
    {
        // Arrange
        var col = new PostgresBoolean(1, ColName, false);
        var colValue = await TestDataReader<bool>(
            GetName(), col, "false,");

        colValue.Should().BeFalse();
    }

    [Fact]
    public async Task TestBoolean_When_Null()
    {
        // Arrange
        var col = new PostgresBoolean(1, ColName, true);
        var colValue = await TestDataReader<bool?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestLargeDecimal()
    {
        // Arrange
        var testVal = 12345678901234567890.123456m;
        var col = new PostgresDecimal(1, ColName, false, 32, 6);
        var colValue = await TestDataReader<decimal>(
            GetName(), col, testVal + ",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestSmallDecimal()
    {
        // Arrange
        var testVal = 1234m;
        var col = new PostgresDecimal(1, ColName, false, 4);
        var colValue = await TestDataReader<decimal>(
            GetName(), col, testVal + ",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestDecimal_When_Null()
    {
        // Arrange
        var col = new PostgresDecimal(1, ColName, true, 20, 2);
        var colValue = await TestDataReader<decimal?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestDoublePrecision()
    {
        // Arrange
        var testVal = 12345678.123456d;
        var col = new PostgresDoublePrecision(1, ColName, false);
        var colValue = await TestDataReader<double>(
            GetName(), col, testVal + ",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestDoublePrecision_When_Null()
    {
        // Arrange
        var col = new PostgresDoublePrecision(1, ColName, true);
        var colValue = await TestDataReader<double?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestMoney()
    {
        // Arrange
        var testVal = 123.12m;
        var col = new PostgresMoney(1, ColName, false);
        var colValue = await TestDataReader<decimal>(
            GetName(), col, testVal + ",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestMoney_When_Null()
    {
        // Arrange
        var col = new PostgresMoney(1, ColName, true);
        var colValue = await TestDataReader<decimal?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }

    [Fact]
    public async Task TestReal()
    {
        // Arrange
        var testVal = 123.12f;
        var col = new PostgresReal(1, ColName, false);
        var colValue = await TestDataReader<float>(
            GetName(), col, testVal + ",");

        colValue.Should().Be(testVal);
    }

    [Fact]
    public async Task TestReal_When_Null()
    {
        // Arrange
        var col = new PostgresReal(1, ColName, true);
        var colValue = await TestDataReader<float?>(
            GetName(), col, ",");

        colValue.Should().BeNull();
    }
}