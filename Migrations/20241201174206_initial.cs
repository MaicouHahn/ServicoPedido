using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicoPedido.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodPedido = table.Column<string>(type: "TEXT", nullable: false),
                    IdFornecedor = table.Column<int>(type: "INTEGER", nullable: false),
                    DataPedido = table.Column<string>(type: "TEXT", nullable: false),
                    IdItem = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pedidos", x => x.IdPedido);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pedidos");
        }
    }
}
