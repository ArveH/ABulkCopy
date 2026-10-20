---
name: add-pg-column-type-tests
description: Add end-to-end round-trip tests (data file → copy in → db assert → copy out → file assert) for a Postgres column type in src/Postgres.Tests/ColumnTypes/. Use when a new Postgres* column class was added to the APostgres provider, when a Pg type lacks ColumnTypes tests, or when the user asks for round-trip/copy in+out tests for a column type.
---

# Add round-trip tests for a Pg column type

One test file per type: `src/Postgres.Tests/ColumnTypes/Test<TypeName>.cs` (e.g. `TestJsonb.cs` for `PostgresJsonb`). Every test runs the full pipeline against a real database: mock-FS `.data` file → `PgDataReader` (copy in) → assert DB value → `DataWriter` + real `PgTableReader` (copy out) → assert file content. Tables are created and dropped per test.

All plumbing lives in `PgColumnTypeTestBase` — new files only pick values and expectations. Reference examples: `TestChar.cs` (string kind, custom multi-row test), `TestDecimal.cs` (number kind), `TestTime.cs` (output format differs from input), `TestByteA.cs` (raw/blob kind).

## The one helper

```csharp
await TestColumnRoundTrip(
    GetName(),                              // table name = test method name; MUST be called in the test method itself (stack-frame based)
    new PostgresFoo(1, ColName, true, ...), // always nullable: true
    inputCell,                              // value exactly as in the .data file; null = null cell. Strings: wrap with QuoteCell(value)
    expectedDbValue,                        // typed value SelectScalar<T> should return; null expects DB null
    expectedOutputCell);                    // omit when copy-out cell == inputCell
```

For null tests, pin T explicitly: `TestColumnRoundTrip<int?>(GetName(), col, null, null)`.

## Standard test set

Always: basic value, null. Then per kind:

- **String-backed** (`PgTemplateStrColumn`): empty string, single quote (`QuoteCell` doubles it in the file), quote at end, only quotes, double quote, comma, backslash at end, unicode (`AllTypes.SampleValues.NVarchar10K` for big), leading spaces, trailing spaces (see gotchas), exact length, 10K value. Write a helper like `TestVarCharRoundTrip` that applies `QuoteCell` + the trailing-space trim default.
- **Numbers**: zero, negative, `MinValue`, `MaxValue`, trailing zeros, scale rounding (Pg rounds to column scale).
- **Date/time**: min/max value, fraction digits, lower precision → rounding, leap day.
- **json/jsonb**: json keeps exact text (whitespace + key order); jsonb normalizes — keys sorted, `{"a":1}` → `{"a": 1}`, duplicate keys last-wins. Assert normalized text as both db and output value.
- **Raw (bytea-like)**: don't use the generic helper for values — mirror `TestByteA.TestByteARoundTrip`: add blob file `FileHelper.DataFolder/<schema>.<table>/Col1/i000000000000000.raw`, data cell = file name, assert DB bytes and the copy-out raw file bytes. Test empty and 1-byte blobs too.

Optionally one multi-row test incl. null mix — see `TestChar_When_MultipleRows`.

## Copy-out formats (expectedOutputCell)

Copy-out cell = `column.ToString(value)` on what **Npgsql returns**, not what went in:

- Strings/char: trailing spaces trimmed, char(n) padding removed → `'Arve  '` comes back as `'Arve'`.
- bool: always `true`/`false` (input `1`/`0` accepted).
- decimal/money: trailing fraction zeros trimmed (`1234.50` → `1234.5`).
- timestamp: always `"O"` format → `2023-06-23T11:12:13.0000000`; timestamptz gets `+00:00` suffix.
- time: `TimeSpan` round-trip format, 7 fraction digits when nonzero → `11:12:13.4567890`.
- uuid: lower case.

## Run

```bash
dotnet test src/Postgres.Tests/Postgres.Tests.csproj --filter "FullyQualifiedName~Postgres.Tests.ColumnTypes.TestFoo"
```

Needs Docker (`Testing:UseContainer=true`) or a Pg connection string in user secrets.

## Gotchas

- `GetName()` reads stack frame 3 — call it directly inside the `[Fact]` method and pass the result down; calling it inside a helper returns the helper's name. Keep method names ≤ 63 chars (Pg identifier limit).
- timestamptz values must be UTC (`Z`) — Npgsql rejects `DateTimeOffset` with nonzero offset on binary import; Npgsql returns timestamptz as UTC `DateTime` on read.
- Quoted cells may contain embedded newlines and commas — both are valid edge tests for text.
- Value-too-long / out-of-range inputs: `PgDataReader` swallows the error and returns a lower row count instead of throwing — don't write such tests; the `rowsRead.Should().Be(1)` in the helper is what catches silent failures.
- Every test leaks nothing: readers/connections are disposed by the base + `DatabaseFixture`; if a run dies with `53300: too many clients`, something new is holding readers/connections open.
- If the type itself isn't wired yet (factory case, `PgTypes`, `GetNativeType`), do that first — see the `add-postgres-type` skill.
