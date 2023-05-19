﻿using System.Data;
using ABulkCopy.Common.Reader;
using ASqlServer;
using Testing.Shared.SqlServer;

namespace Common.Test;

public class DataWriterTests
{
    private const string TestPath = @"C:\testfiles";
    private readonly string _testTableName = Environment.MachineName + "DataWriterTests";
    private readonly ILogger _logger;
    private readonly TableDefinition _originalTableDefinition;
    private readonly MockFileSystem _mockFileSystem;
    private readonly DataWriter _dataWriter;

    public DataWriterTests(ITestOutputHelper output)
    {
        _logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(output)
            .CreateLogger();

        _originalTableDefinition = MssTestData.GetEmpty(_testTableName);
        _mockFileSystem = new MockFileSystem();
        _mockFileSystem.AddDirectory(TestPath);
        _dataWriter = new DataWriter(_mockFileSystem, _logger);
    }

    [Fact]
    public async Task TestWriteBigInt()
    {
        // Arrange
        var col = new SqlServerBigInt(101, "MyTestCol", false);
        _originalTableDefinition.Columns.Add(col);
        await MssDbHelper.Instance.DropTable(_testTableName);
        await MssDbHelper.Instance.CreateTable(_originalTableDefinition);
        ITableReader tableReader = new MssTableReader(
            new SelectCreator(_logger), _logger)
        {
            ConnectionString = MssDbHelper.Instance.ConnectionString
        };

        // Act
        try
        {
            await _dataWriter.WriteTable(
                tableReader,
                _originalTableDefinition,
                TestPath);
        }
        finally
        {
            await MssDbHelper.Instance.DropTable(_testTableName);
        }

        // Assert
        var jsonTxt = await GetJsonText();
    }

    private async Task<string> GetJsonText()
    {
        var fullPath = Path.Combine(TestPath, _testTableName + CommonConstants.DataSuffix);
        _mockFileSystem.FileExists(fullPath).Should().BeTrue("because data file should exist");
        var jsonTxt = await _mockFileSystem.File.ReadAllTextAsync(fullPath);
        return jsonTxt;
    }
}