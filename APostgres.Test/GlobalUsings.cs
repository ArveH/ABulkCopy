global using ABulkCopy.APostgres;
global using ABulkCopy.APostgres.Column;
global using ABulkCopy.APostgres.Column.ColumnTypes;
global using ABulkCopy.APostgres.Mapping;
global using ABulkCopy.APostgres.Reader;
global using ABulkCopy.ASqlServer.Column.ColumnTypes;
global using ABulkCopy.Common;
global using ABulkCopy.Common.Config;
global using ABulkCopy.Common.Database;
global using ABulkCopy.Common.Identifier;
global using ABulkCopy.Common.Mapping;
global using ABulkCopy.Common.Reader;
global using ABulkCopy.Common.TestData;
global using ABulkCopy.Common.Types;
global using ABulkCopy.Common.Types.Column;
global using ABulkCopy.Common.Types.Table;
global using ABulkCopy.Common.Utils;
global using AParser.Parsers.Pg;
global using FluentAssertions;
global using Microsoft.Extensions.Configuration;
global using Moq;
global using Serilog;
global using System.Diagnostics;
global using System.IO.Abstractions.TestingHelpers;
global using Testing.Shared;
global using Testing.Shared.Postgres;
global using Xunit;
global using Xunit.Abstractions;