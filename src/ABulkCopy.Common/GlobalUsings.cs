// Global using directives

global using ABulkCopy.Common.Config;
global using ABulkCopy.Common.Database;
global using ABulkCopy.Common.Exceptions;
global using ABulkCopy.Common.Extensions;
global using ABulkCopy.Common.Graph.Visitors;
global using ABulkCopy.Common.Reader;
global using ABulkCopy.Common.Serialization;
global using ABulkCopy.Common.Types;
global using ABulkCopy.Common.Types.Column;
global using ABulkCopy.Common.Types.Index;
global using ABulkCopy.Common.Types.Table;
global using Microsoft.Extensions.Configuration;
global using Serilog;
global using System.Collections.Concurrent;
global using System.IO.Abstractions;
global using System.Numerics;
global using System.Text;
global using System.Text.Encodings.Web;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Text.RegularExpressions;
global using SchemaTableTuple = (string schemaName, string tableName);