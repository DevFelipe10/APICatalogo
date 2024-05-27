using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogoxUnitTests.UnitTests.Produtos;

public class GetProdutosUnitTests : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public GetProdutosUnitTests(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task GetProdutoById_OkResult()
    {
        //Arrange
        var prodId = 3;

        //Act
        var data = await _controller.Get(prodId);

        //Assert (xunit)
        //var okResult = Assert.IsType<OkObjectResult>(data.Result);
        //Assert.Equal(200, okResult.StatusCode);

        //Assert (fluentassertions)
        data.Result.Should().BeOfType<OkObjectResult>()
                   .Which.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task GetProdutoById_NotFound()
    {
        //Arrange
        var prodId = 999;

        //Act
        var data = await _controller.Get(prodId);

        //Assert
        data.Result.Should().BeOfType<NotFoundObjectResult>()
                   .Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetProdutoById_BadRequest()
    {
        //Arrange
        var prodId = -1;

        //Act
        var data = await _controller.Get(prodId);

        //Assert
        data.Result.Should().BeOfType<BadRequestObjectResult>()
                   .Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task GetProdutos_Return_ListOfProdutoDTO()
    {
        //Act
        var data = await _controller.Get();

        //Assert
        data.Result.Should().BeOfType<OkObjectResult>()
                    .Which.Value.Should().BeAssignableTo<IEnumerable<ProdutoDTO>>()
                    .And.NotBeNull();
    }

    [Fact]
    public async Task GetProdutos_Return_BadRequestResult()
    {
        //Act
        var data = await _controller.Get();

        //Assert
        data.Result.Should().BeOfType<BadRequestResult>();
    }
}
