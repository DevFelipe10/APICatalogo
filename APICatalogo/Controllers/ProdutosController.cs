using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _repository;

    public ProdutosController(IProdutoRepository produtoRepository)
    {
        _repository = produtoRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _repository.GetProdutos().ToList();

        if (produtos is null)
        {
            return NotFound("Produtos não encontrados...");
        }

        return Ok(produtos);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _repository.GetProduto(id);

        if (produto is null)
        {
            return NotFound($"Produto com id = {id} não encontrado...");
        }

        return Ok(produto);
    }

    [HttpPost]
    public ActionResult<Produto> Post(Produto produto)
    {
        if (produto is null)
            return BadRequest("Objeto produto é nulo");

        var novoProduto = _repository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto",
            new { id = novoProduto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Produto> Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return BadRequest($"O id = {id} informado não é igual ao id de produto");
        }

        bool atualizado = _repository.Update(produto);

        if (atualizado)
        {
            return Ok(produto);
        }
        else
        {
            return StatusCode(500, $"Falha ao atualizar o produto de id = {id}");
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Produto> Delete(int id)
    {
        bool deletado = _repository.Delete(id);

        if (deletado)
        {
            return Ok($"Produto de id = {id} foi excluído");
        }
        else
        {
            return StatusCode(500, $"Falha ao excluir o produto de id = {id}");
        }
    }
}
