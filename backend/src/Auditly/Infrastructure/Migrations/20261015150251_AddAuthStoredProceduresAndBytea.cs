using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthStoredProceduresAndBytea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // PasswordHash konvertálása text-ről bytea-ra a USING kulcsszóval
            migrationBuilder.Sql(@"
                ALTER TABLE ""Users"" 
                ALTER COLUMN ""PasswordHash"" TYPE BYTEA 
                USING (""PasswordHash""::bytea);
            ");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Specializations",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Specializations",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "Entrepreneurs",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Entrepreneurs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Entrepreneurs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "Accountants",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Accountants",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Accountants",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "text", nullable: true),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevokedByIp = table.Column<string>(type: "text", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VerificationToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerificationToken_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entrepreneurs_CompanyName",
                table: "Entrepreneurs",
                column: "CompanyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entrepreneurs_Iban",
                table: "Entrepreneurs",
                column: "Iban",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entrepreneurs_Phone",
                table: "Entrepreneurs",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entrepreneurs_TaxNumber",
                table: "Entrepreneurs",
                column: "TaxNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_CompanyName",
                table: "Accountants",
                column: "CompanyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_Phone",
                table: "Accountants",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accountants_RegistrationNumber",
                table: "Accountants",
                column: "RegistrationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationToken_UserId",
                table: "VerificationToken",
                column: "UserId");

            // Tárolt eljárások létrehozása
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION register_entrepreneur(
                    p_full_name VARCHAR,
                    p_email VARCHAR,
                    p_password_hash BYTEA,
                    p_password_salt BYTEA,
                    p_role INT,
                    p_company_name VARCHAR,
                    p_tax_number VARCHAR,
                    p_iban VARCHAR,
                    p_phone VARCHAR,
                    p_ver_token VARCHAR,
                    p_ver_expires TIMESTAMP WITH TIME ZONE
                ) RETURNS INT AS $$
                DECLARE
                    v_user_id INT;
                BEGIN
                    INSERT INTO ""Users"" (""FullName"", ""Email"", ""PasswordHash"", ""PasswordSalt"", ""Role"", ""IsDeleted"", ""CreatedAt"", ""UpdatedAt"")
                    VALUES (p_full_name, p_email, p_password_hash, p_password_salt, p_role, FALSE, NOW(), NOW())
                    RETURNING ""Id"" INTO v_user_id;

                    INSERT INTO ""Entrepreneurs"" (""UserId"", ""CompanyName"", ""TaxNumber"", ""Iban"", ""Phone"")
                    VALUES (v_user_id, p_company_name, p_tax_number, p_iban, p_phone);

                    INSERT INTO ""VerificationTokens"" (""Token"", ""Type"", ""ExpiresAt"", ""UserId"", ""IsUsed"")
                    VALUES (p_ver_token, 0, p_ver_expires, v_user_id, FALSE);

                    RETURN v_user_id;
                END;
                $$ LANGUAGE plpgsql;
            ");

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION get_user_for_login(p_email VARCHAR)
                RETURNS TABLE (
                    id INT,
                    email VARCHAR,
                    password_hash BYTEA,
                    password_salt BYTEA,
                    role INT
                ) AS $$
                BEGIN
                    RETURN QUERY 
                    SELECT u.""Id"", u.""Email"", u.""PasswordHash"", u.""PasswordSalt"", CAST(u.""Role"" AS INT)
                    FROM ""Users"" u 
                    WHERE u.""Email"" = p_email AND u.""IsDeleted"" = FALSE;
                END;
                $$ LANGUAGE plpgsql;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS register_entrepreneur(VARCHAR, VARCHAR, BYTEA, BYTEA, INT, VARCHAR, VARCHAR, VARCHAR, VARCHAR, VARCHAR, TIMESTAMP WITH TIME ZONE);");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_user_for_login(VARCHAR);");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "VerificationToken");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Entrepreneurs_CompanyName",
                table: "Entrepreneurs");

            migrationBuilder.DropIndex(
                name: "IX_Entrepreneurs_Iban",
                table: "Entrepreneurs");

            migrationBuilder.DropIndex(
                name: "IX_Entrepreneurs_Phone",
                table: "Entrepreneurs");

            migrationBuilder.DropIndex(
                name: "IX_Entrepreneurs_TaxNumber",
                table: "Entrepreneurs");

            migrationBuilder.DropIndex(
                name: "IX_Accountants_CompanyName",
                table: "Accountants");

            migrationBuilder.DropIndex(
                name: "IX_Accountants_Phone",
                table: "Accountants");

            migrationBuilder.DropIndex(
                name: "IX_Accountants_RegistrationNumber",
                table: "Accountants");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Entrepreneurs");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Users"" 
                ALTER COLUMN ""PasswordHash"" TYPE TEXT 
                USING (""PasswordHash""::text);
            ");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Specializations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Specializations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                table: "Entrepreneurs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(13)",
                oldMaxLength: 13);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Entrepreneurs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "Accountants",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Accountants",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyName",
                table: "Accountants",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);
        }
    }
}