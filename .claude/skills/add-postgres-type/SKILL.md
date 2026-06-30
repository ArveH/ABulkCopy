---
name: add-postgres-type
description: Add support for a new Postgres column type (e.g. json, jsonb, interval, inet) to the ABulkCopy Postgres provider so it can be copied out/in. Use when adding a Postgres data type, a schema/data read throws ArgumentOutOfRangeException for an unknown nativeType, or the user wants ABulkCopy to handle a Pg type it currently rejects.
---

# Add a new Postgres type

Adds one Postgres type to the `Pg*` provider so `CopyOut`/`CopyIn` handle it.
`Mss` = SQL Server, `Pg` = Postgres. Reference: the json/jsonb work (string-backed type, mirrors SQL Server `xml`).

## Pick a base class

Behavior of the type decides the base for the new column class (`src/ABulkCopy.APostgres/Column/ColumnTypes/`):

| Type behaves like | Base class | Example |
|-------------------|-----------|---------|
| string / text | `PgTemplateStrColumn` | `PostgresJson`, `PostgresText` |
| integer | `PgTemplateNumberColumn` | `PostgresInt` |
| anything else | `PgDefaultColumn` + override `ToString`/`ToInternalType`/`GetDotNetType` | `PostgresDecimal`, `PostgresUuid` |

`ToString(value)` = serialize DB value → `.data` file (copy-out).
`ToInternalType(string)` = `.data` string → value for Npgsql (copy-in).
`GetDotNetType()` = CLR type Npgsql returns.
`GetTypeClause()` = the `CREATE TABLE` type fragment (e.g. `"jsonb"`, `"varchar(255)"`).

## Steps (checklist)

1. **`src/ABulkCopy.Common/Types/Column/PgTypes.cs`** — add `public const string Foo = "foo";` (string = Postgres `typname` from `pg_type`, lowercase). Add to `AllTypes` list.
2. **`src/ABulkCopy.APostgres/Column/ColumnTypes/PostgresFoo.cs`** — new column class on chosen base. Don't right-trim values where trailing space is meaningful (json passed `shouldTrim: false`).
3. **`src/ABulkCopy.APostgres/Column/PgColumnFactory.cs`** — add `PgTypes.Foo => new PostgresFoo(...)` to the switch. The factory receives the `typname` from `PgSystemTables.GetColumnInfo`. Add every alias (e.g. `int8`/`bigint`, `float4`/`real`).
4. **`src/ABulkCopy.APostgres/Extensions/StringExtensions.cs`** — add `PgTypes.Foo => NpgsqlDbType.Foo` to `GetNativeType` (used by copy-in binary import).
5. **`src/Postgres.Tests/SystemTables/ColumnInfo/PgTestFooColumn.cs`** — integration test (DB-backed). Mirror `PgTestJsonColumn`: create table with the column, `GetTableColumnInfoAsync`, assert `Type`, `GetTypeClause()`, `GetDotNetType()`, `ToString(...)` output.

## Verify

```bash
dotnet build src/ABulkCopy.sln
dotnet test src/Postgres.Tests/Postgres.Tests.csproj --filter "FullyQualifiedName~PgTestFooColumn"
```

DB tests need Docker (`Testing:UseContainer=true`) or a reachable Pg connection string in user secrets.

## Gotchas

- The factory's `default` branch throws `ArgumentOutOfRangeException(nativeType)` — a missing case there is the usual symptom of an unsupported type.
- `length`/`precision`/`scale` come from the column-info query; pass them through only if the type clause needs them. Variable-length types report `attlen = -1`.
- Full Pg copy-**out** is also gated by `TableReaderFactory` (only `MssTableReader` wired today). Column-type handling is independent of that — adding the type makes schema read + data serialization correct regardless.
- For a string-backed type, extending `PgTemplateStrColumn` gives `ToInternalType`/`GetDotNetType` free; override only `ToString` (trim) and `GetTypeClause`.
