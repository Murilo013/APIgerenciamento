using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiGBM.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "caminhao",
                columns: table => new
                {
                    cd_Placa = table.Column<string>(type: "text", nullable: false),
                    ds_Modelo = table.Column<string>(type: "text", nullable: false),
                    aa_Fabricacao = table.Column<int>(type: "integer", nullable: false),
                    ds_CorPrincipal = table.Column<string>(type: "text", nullable: false),
                    qt_EixosTracao = table.Column<int>(type: "integer", nullable: false),
                    fk_CPFmotorista = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_caminhao", x => x.cd_Placa);
                });

            migrationBuilder.CreateTable(
                name: "entrega",
                columns: table => new
                {
                    cd_Entrega = table.Column<Guid>(type: "uuid", nullable: false),
                    dt_Entrega = table.Column<string>(type: "text", nullable: false),
                    ds_Origem = table.Column<string>(type: "text", nullable: false),
                    ds_Destino = table.Column<string>(type: "text", nullable: false),
                    ds_CargaTransportada = table.Column<string>(type: "text", nullable: false),
                    ds_StatusEntrega = table.Column<string>(type: "text", nullable: false),
                    fk_PlacaCaminhao = table.Column<string>(type: "text", nullable: false),
                    fk_CPFmotorista = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entrega", x => x.cd_Entrega);
                });

            migrationBuilder.CreateTable(
                name: "motorista",
                columns: table => new
                {
                    cd_CPF = table.Column<string>(type: "text", nullable: false),
                    nm_Nome = table.Column<string>(type: "text", nullable: false),
                    ds_CategoriaCNH = table.Column<string>(type: "text", nullable: false),
                    dt_Nascimento = table.Column<string>(type: "text", nullable: false),
                    ds_Telefone = table.Column<string>(type: "text", nullable: false),
                    fk_PlacaCaminhao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motorista", x => x.cd_CPF);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "caminhao");

            migrationBuilder.DropTable(
                name: "entrega");

            migrationBuilder.DropTable(
                name: "motorista");
        }
    }
}
