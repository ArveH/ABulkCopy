// Global using directives

global using ABulkCopy.APostgres;
global using ABulkCopy.ASqlServer;
global using ABulkCopy.ASqlServer.Column.ColumnTypes;
global using ABulkCopy.Cmd.Internal;
global using ABulkCopy.Common;
global using ABulkCopy.Common.Config;
global using ABulkCopy.Common.Database;
global using ABulkCopy.Common.Extensions;
global using ABulkCopy.Common.Identifier;
global using ABulkCopy.Common.Types;
global using ABulkCopy.Common.Types.Column;
global using ABulkCopy.Common.Types.Table;
global using CrossRDBMS.Tests.Helpers;
global using FluentAssertions;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Npgsql;
global using Serilog;
global using Serilog.Sinks.ListOfString;
global using System.Diagnostics;
global using System.IO.Abstractions;
global using System.IO.Abstractions.TestingHelpers;
global using Testcontainers.MsSql;
global using Testcontainers.PostgreSql;
global using Testing.Shared.SqlServer;
global using Xunit.Abstractions;
global using SchemaTableTuple = (string schemaName, string tableName);