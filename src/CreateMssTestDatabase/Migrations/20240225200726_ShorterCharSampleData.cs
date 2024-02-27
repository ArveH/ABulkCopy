using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreateMssTestDatabase.Migrations
{
    /// <inheritdoc />
    public partial class ShorterCharSampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AllTypes",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CharStrChar20", "CharStrNChar20", "DTDateTime", "DTDateTime2", "DTDateTimeOffset", "DTSmallDateTime" },
                values: new object[] { "1234567890", "ﯵ1234567890", new DateTime(2023, 3, 31, 11, 12, 13, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 31, 11, 12, 13, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2023, 3, 31, 11, 12, 13, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), new DateTime(2023, 3, 31, 11, 12, 13, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AllTypes",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CharStrChar20", "CharStrNChar20", "DTDateTime", "DTDateTime2", "DTDateTimeOffset", "DTSmallDateTime" },
                values: new object[] { "12345678901234567890", "123456789ﯵ1234567890", new DateTime(2023, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTimeOffset(new DateTime(2023, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), new DateTime(2023, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
