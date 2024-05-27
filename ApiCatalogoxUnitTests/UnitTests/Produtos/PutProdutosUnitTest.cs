using APICatalogo.Controllers;
using APICatalogo.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTests.Produtos;

public class PutProdutosUnitTest : IClassFixture<ProdutosUnitTestController>
{
    private readonly ProdutosController _controller;

    public PutProdutosUnitTest(ProdutosUnitTestController controller)
    {
        _controller = new ProdutosController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task PutProduto_Return_OkResult()
    {
        int id = 2;
        ProdutoDTO produtoDTO = new ProdutoDTO
        {
            ProdutoId = id,
            CategoriaId = 3,
            Descricao = "Isso é um teste",
            ImagemUrl = "image.jpeg",
            Nome = "Produto Teste",
            Preco = 10.90m,
        };

        var result = await _controller.Put(id, produtoDTO);

        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>()
                    .Subject.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task PutProduto_Return_BadRequest()
    {
        int id = 2;
        ProdutoDTO produtoDTO = new ProdutoDTO
        {
            ProdutoId = 99,
            CategoriaId = 3,
            Descricao = "Isso é um teste",
            ImagemUrl = "image.jpeg",
            Nome = "Produto Teste",
            Preco = 10.90m,
        };

        var result = await _controller.Put(id, produtoDTO);

        result.Result.Should().BeOfType<BadRequestObjectResult>()
                            .Which.StatusCode.Should().Be(400);
    }
}
