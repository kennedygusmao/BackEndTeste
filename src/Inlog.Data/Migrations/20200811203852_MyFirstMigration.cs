using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inlog.Data.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: true),
                    DataCadastro = table.Column<DateTime>(nullable: true),
                    Chassi = table.Column<string>(type: "varchar(200)", nullable: false),
                    TipoVeiculo = table.Column<int>(nullable: false),
                    NumeroPassageiros = table.Column<byte>(nullable: false),
                    Cor = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Veiculos");
        }
    }
}
