using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("insert into Categorias(Nome, ImagemUrl) values('Bebidas', 'bebidas.jpg')");
            mb.Sql("insert into Categorias(Nome, ImagemUrl) values('Lanches', 'lanches.jpg')");
            mb.Sql("insert into Categorias(Nome, ImagemUrl) values('Sobremesas', 'sobremesas.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
        }
    }
}
