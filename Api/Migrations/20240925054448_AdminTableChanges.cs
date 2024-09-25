using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace minimal_api.Migrations
{
    /// <inheritdoc />
    public partial class AdminTableChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ano",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Vehicles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Marca",
                table: "Vehicles",
                newName: "CarBrand");

            migrationBuilder.RenameColumn(
                name: "Senha",
                table: "Administrators",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "Perfil",
                table: "Administrators",
                newName: "Profile");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Vehicles",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "CarBrand",
                table: "Vehicles",
                newName: "Marca");

            migrationBuilder.RenameColumn(
                name: "Profile",
                table: "Administrators",
                newName: "Perfil");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Administrators",
                newName: "Senha");

            migrationBuilder.AddColumn<string>(
                name: "Ano",
                table: "Vehicles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
