﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace boticario.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BOTICARIO");

            migrationBuilder.CreateTable(
                name: "ParametroSistema",
                schema: "BOTICARIO",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeParametro = table.Column<string>(nullable: false),
                    Valor = table.Column<string>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false, defaultValue: true),
                    DataCriacao = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    DataAlteracao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParametroSistema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegraCashback",
                schema: "BOTICARIO",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Inicio = table.Column<int>(nullable: false),
                    Fim = table.Column<int>(nullable: false),
                    Percentual = table.Column<int>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false, defaultValue: true),
                    DataCriacao = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    DataAlteracao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegraCashback", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Revendedor",
                schema: "BOTICARIO",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: false),
                    Cpf = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Senha = table.Column<string>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false, defaultValue: true),
                    DataCriacao = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    DataAlteracao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revendedor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusCompra",
                schema: "BOTICARIO",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false, defaultValue: true),
                    DataCriacao = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    DataAlteracao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusCompra", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoHistorico",
                schema: "BOTICARIO",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false, defaultValue: true),
                    DataCriacao = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    DataAlteracao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoHistorico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Compra",
                schema: "BOTICARIO",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRevendedor = table.Column<int>(nullable: false),
                    RevendedorId = table.Column<int>(nullable: true),
                    IdStatus = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: true),
                    Codigo = table.Column<string>(nullable: true),
                    Valor = table.Column<int>(nullable: false),
                    DataCompra = table.Column<DateTime>(nullable: false),
                    PercentualCashback = table.Column<int>(nullable: false),
                    ValorCashback = table.Column<int>(nullable: false),
                    Ativo = table.Column<bool>(nullable: false, defaultValue: true),
                    DataCriacao = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    DataAlteracao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compra_Revendedor_RevendedorId",
                        column: x => x.RevendedorId,
                        principalSchema: "BOTICARIO",
                        principalTable: "Revendedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compra_StatusCompra_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "BOTICARIO",
                        principalTable: "StatusCompra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Historico",
                schema: "BOTICARIO",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTipoHistorico = table.Column<int>(nullable: false),
                    TipoHistoricoId = table.Column<int>(nullable: true),
                    Usuario = table.Column<string>(nullable: false),
                    NomeTabela = table.Column<string>(nullable: false),
                    ChaveTabela = table.Column<int>(nullable: false),
                    JsonAntes = table.Column<string>(nullable: true),
                    JsonDepois = table.Column<string>(nullable: true),
                    Data = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historico_TipoHistorico_TipoHistoricoId",
                        column: x => x.TipoHistoricoId,
                        principalSchema: "BOTICARIO",
                        principalTable: "TipoHistorico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compra_RevendedorId",
                schema: "BOTICARIO",
                table: "Compra",
                column: "RevendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_StatusId",
                schema: "BOTICARIO",
                table: "Compra",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Historico_TipoHistoricoId",
                schema: "BOTICARIO",
                table: "Historico",
                column: "TipoHistoricoId");

            migrationBuilder.CreateIndex(
                name: "IX_ParametroSistema_NomeParametro",
                schema: "BOTICARIO",
                table: "ParametroSistema",
                column: "NomeParametro",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Revendedor_Cpf",
                schema: "BOTICARIO",
                table: "Revendedor",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Revendedor_Email",
                schema: "BOTICARIO",
                table: "Revendedor",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusCompra_Descricao",
                schema: "BOTICARIO",
                table: "StatusCompra",
                column: "Descricao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoHistorico_Descricao",
                schema: "BOTICARIO",
                table: "TipoHistorico",
                column: "Descricao",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compra",
                schema: "BOTICARIO");

            migrationBuilder.DropTable(
                name: "Historico",
                schema: "BOTICARIO");

            migrationBuilder.DropTable(
                name: "ParametroSistema",
                schema: "BOTICARIO");

            migrationBuilder.DropTable(
                name: "RegraCashback",
                schema: "BOTICARIO");

            migrationBuilder.DropTable(
                name: "Revendedor",
                schema: "BOTICARIO");

            migrationBuilder.DropTable(
                name: "StatusCompra",
                schema: "BOTICARIO");

            migrationBuilder.DropTable(
                name: "TipoHistorico",
                schema: "BOTICARIO");
        }
    }
}
