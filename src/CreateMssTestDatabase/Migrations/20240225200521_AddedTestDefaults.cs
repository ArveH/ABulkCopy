using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreateMssTestDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddedTestDefaults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE DEFAULT df_num_default AS 0");

            migrationBuilder.Sql("CREATE TABLE TestDefaults (\r\n" +
                                 "  id INTEGER,\r\n" +
                                 "  int1 INT NOT NULL DEFAULT 0,\r\n" +
                                 "  int2 INT NOT NULL CONSTRAINT df_bulkcopy_int DEFAULT 0,\r\n" +
                                 "  int3 INT NOT NULL,\r\n" +
                                 "  date1 DATETIME2 NOT NULL DEFAULT GETDATE()\r\n)");

            migrationBuilder.Sql("sp_bindefault 'df_num_default', 'TestDefaults.int3'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("TestDefaults");

            migrationBuilder.Sql("DROP DEFAULT df_num_default");
        }

    }
}
