// Global using directives

global using ABulkCopy.APostgres;
global using ABulkCopy.APostgres.Column;
global using ABulkCopy.APostgres.Mapping;
global using ABulkCopy.APostgres.Reader;
global using ABulkCopy.ASqlServer;
global using ABulkCopy.ASqlServer.Column;
global using ABulkCopy.ASqlServer.Table;
global using ABulkCopy.Common;
global using ABulkCopy.Common.Config;
global using ABulkCopy.Common.Database;
global using ABulkCopy.Common.Extensions;
global using ABulkCopy.Common.Graph;
global using ABulkCopy.Common.Identifier;
global using ABulkCopy.Common.Mapping;
global using ABulkCopy.Common.Reader;
global using ABulkCopy.Common.Types;
global using ABulkCopy.Common.Types.Table;
global using ABulkCopy.Common.Utils;
global using ABulkCopy.Common.Writer;
global using AParser;
global using AParser.Parsers.Pg;
global using CommandLine;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Serilog;
global using System.Diagnostics;
global using System.IO.Abstractions;
global using System.Text.RegularExpressions;
global using Serilog.Settings.Configuration;