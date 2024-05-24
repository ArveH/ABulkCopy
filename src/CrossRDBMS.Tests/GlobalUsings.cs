// Global using directives

global using ABulkCopy.APostgres;
global using ABulkCopy.ASqlServer;
global using ABulkCopy.Cmd.Internal;
global using ABulkCopy.Common;
global using ABulkCopy.Common.Config;
global using ABulkCopy.Common.Database;
global using ABulkCopy.Common.Identifier;
global using ABulkCopy.Common.Types;
global using CommandLine;
global using FluentAssertions;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Npgsql;
global using System.Data.SqlClient;
global using System.Diagnostics;
global using System.IO.Abstractions;
global using CrossRDBMS.Tests.Helpers;
global using Serilog;
global using Serilog.Sinks.ListOfString;
global using Testcontainers.MsSql;
global using Testcontainers.PostgreSql;
global using Xunit.Abstractions;