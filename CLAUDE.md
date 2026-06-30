# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

ABulkCopy is a .NET 8 CLI tool that copies database tables (schema + data) between
RDBMSs via an intermediate file format. You run it **twice**: once to copy *out* of a
source DB into `.schema`/`.data` files on disk, once to copy *in* from those files into a
destination DB. Currently supported path: **copy out from SQL Server → copy in to Postgres**.
The longer-term goal is any-to-any (Mss↔Pg, same-to-same).

The two-phase file-based design is the central architectural fact: `.schema` files (JSON
table/column/index definitions) and `.data` files are the contract between the export and
import halves, and they also drive the dependency graph used to order table creation.

## Build, run, test

```bash
# Build (solution is under src/)
dotnet build src/ABulkCopy.sln

# Run the CLI
dotnet run --project src/ABulkCopy.Cmd -- --help

# Run all tests
dotnet test src/ABulkCopy.sln

# Run one test project / a single test
dotnet test src/SqlServer.Tests/SqlServer.Tests.csproj
dotnet test src/ABulkCopy.sln --filter "FullyQualifiedName~MssReaderTests"
```

### Database integration tests

`SqlServer.Tests`, `Postgres.Tests`, `APostgres.Tests`, `ASqlServer.Tests`, and
`CrossRDBMS.Tests` talk to a real database. The `DatabaseFixture` in each decides at runtime:

- If config key `Testing:UseContainer` is true → it spins up a **Testcontainers** SQL
  Server / Postgres instance (Docker required).
- Otherwise it uses a connection string from configuration / **user secrets** (each test
  project has its own user-secrets id, set in `ConfigHelper.GetConfiguration(...)`).

So to run DB tests you either need Docker running with `UseContainer=true`, or a reachable
DB with the connection string in user secrets. `AParser.Tests` and `Common.Tests` are pure
unit tests with no DB dependency.

## How a run is wired

`ABulkCopy.Cmd/Program.cs` is the entry point:
1. `CmdArguments.Create(args)` parses the command line (CommandLineParser library) into
   `CmdArguments`, then `ToAppSettings()` flattens selected args into an in-memory config
   dictionary layered on top of `appsettings.json` + user secrets.
2. `ConfigureServices(rdbms, configuration)` registers DI services. **The RDBMS argument
   selects which provider package's services are registered** — `AddMssServices()` or
   `AddPgServices()`.
3. Optional `--pre-script` runs (via `IScriptRunner`), then either `ICopyOut.RunAsync`
   (Direction=Out) or `ICopyIn.RunAsync` (Direction=In), then optional `--post-script`.

`CopyOut` (export) reads table list from `IMssSystemTables`, gets schema via
`IMssTableSchema`, and writes files with `ISchemaWriter`/`IDataWriter` — parallelised per
table. `CopyIn` (import) globs `*.schema` files, builds a dependency graph, orders tables
with `ITableSequencer`, then for each table drops/creates it, bulk-inserts data, resets
identity columns, and creates indexes. **Note:** `CopyOut` is currently hard-wired to the
`IMss*` (SQL Server) services and `CopyIn` to the `IPg*` (Postgres) services — this is the
concrete reason only Mss-out / Pg-in works today. Generalising means abstracting these.

## Project layout (src/)

- **ABulkCopy.Cmd** — console host, `Program.cs`, `appsettings.json`, sample mapping/script files.
- **ABulkCopy.Cmd.Internal** — orchestration: `CopyIn`, `CopyOut`, arg parsing, DI
  registration (`HostApplicationBuilderExtensions`), provider factories.
- **ABulkCopy.Common** — provider-agnostic core: type model (`Types/` — `TableDefinition`,
  `Column`, `Index`), schema/data `Writer` + `Reader`, dependency `Graph` (`DependencyGraph`,
  `TableSequencer`), `Scripts` runner, `Identifier` quoting, JSON `Serialization`.
- **ABulkCopy.ASqlServer** — SQL Server provider (`Mss*` types: system-table queries,
  schema reading, query building, raw commands).
- **ABulkCopy.APostgres** — Postgres provider (`Pg*` types: bulk copy, cmd, system tables,
  type mapping).
- **AParser** — a hand-written SQL tokenizer/parser (`Tokenizer`, `Parsers`, `Tree`) used to
  interpret SQL Server default-clause expressions (e.g. `convert(...)`) so they can be
  translated. Deliberately narrow scope — see `AParser/SupportedSyntax.md` for the EBNF
  grammar of what it actually handles.
- **Testing.Shared** — shared test helpers (file system, SQL Server helpers).
- **\*.Tests** — `Common.Tests`, `AParser.Tests` (unit); `SqlServer.Tests`, `Postgres.Tests`,
  `ASqlServer.Tests`, `APostgres.Tests`, `CrossRDBMS.Tests` (DB integration via fixtures);
  `Benchmark.Test` (BenchmarkDotNet).

## Naming convention

`Mss` = Microsoft SQL Server, `Pg` = Postgres. The `A` prefix (AParser, APostgres,
ASqlServer, ADataReader) is the author's project namespacing — not a separate concept.
`Rdbms` enum values are `Mss` and `Pg`; `CopyDirection` is `In`/`Out`.

## Key CLI parameters (full list via `--help`)

Required: `-d/--direction` (In/Out), `-r/--rdbms` (Mss/Pg), `-c/--connection-string`.
Common optional: `-f/--folder` (file location), `-m/--mappings-file` (schema/collation/type
name mapping, In only), `-q/--add-quotes` (Postgres identifier quoting), `--schema-filter`
(Out only), `-s/--search-filter` (regex when In, SQL `LIKE` rhs when Out), `--skip-create`
(experimental: insert into EF-migration-created tables), `--pre-script`/`--post-script`.
Many flags only apply to one direction — check the `HelpText` in
`ABulkCopy.Cmd.Internal/CmdArguments.cs`, which is the source of truth for parameter behaviour.

## Stack notes

- .NET 8, generic host (`Host.CreateApplicationBuilder`) + Microsoft.Extensions DI.
- Serilog for logging (file + configured sinks); config in `appsettings.json` `Serilog` section.
- `System.IO.Abstractions` (`IFileSystem`) is injected everywhere file IO happens, so tests
  use an in-memory file system — don't reach for `System.IO.File` directly.
- Tests: xUnit + FluentAssertions + Moq + Testcontainers.
- Versioning via GitVersion (`GitVersion.yml`); publish targets win-x64/linux-x64/linux-arm64/osx-arm64.

## Full docs

External documentation: https://arveh.github.io/ABulkCopy.Docs/
