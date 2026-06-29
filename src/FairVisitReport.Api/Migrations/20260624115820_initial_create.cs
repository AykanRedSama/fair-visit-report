using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FairVisitReport.Api.Migrations
{
    /// <inheritdoc />
    public partial class initial_create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "visit_reports",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    position = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    company = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    mail_address = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    report_text = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    exported = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    exported_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visit_reports", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "visit_reports");
        }
    }
}
