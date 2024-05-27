using APICatalogo.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTests.Produtos;

public class DeleteProdutosUnitTest : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public DeleteProdutosUnitTest(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task DeleteProdutoById_Return_OkResult()
    {
        var prodId = 3;

        var result = await _controller.Delete(prodId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
    }


    [Fact]
    public async Task DeleteProdutoById_Return_NotFound()
    {
        var prodId = 9999;

        var result = await _controller.Delete(prodId);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }
}
