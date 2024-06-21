using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desafio_Loja_de_Carros.Migrations
{
    /// <inheritdoc />
    public partial class tudoindobem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ComissaoTotal",
                table: "Vendedor",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComissaoTotal",
                table: "Vendedor");
        }
    }
}
