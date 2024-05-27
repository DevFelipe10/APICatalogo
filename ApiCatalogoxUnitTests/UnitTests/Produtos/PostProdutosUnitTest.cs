using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests.Produtos;

public class PostProdutosUnitTest : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public PostProdutosUnitTest(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task PostProduto_Return_CreatedStatusCode()
    {
        //Arrange
        var novoProdutoDto = new ProdutoDTO
        {
            Nome = "Novo Produto",
            Descricao = "Descirção 123",
            Preco = 10.99m,
            ImagemUrl = "image.png",
            CategoriaId = 3
        };

        //Act
        var data = await _controller.Post(novoProdutoDto);

        //Assert
        var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
        createdResult.Subject.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task PostProduto_Return_BadRequestStatusCode()
    {
        //Arrange
        ProdutoDTO? novoProdutoDto = null;

        //Act
        var data = await _controller.Post(novoProdutoDto);

        //Assert
        var createdResult = data.Result.Should().BeOfType<BadRequestObjectResult>();
        createdResult.Subject.StatusCode.Should().Be(400);
    }
}
