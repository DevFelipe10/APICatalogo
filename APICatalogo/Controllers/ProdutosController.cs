using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Get()
    {
        var produtos = await _context.Produtos
            .AsNoTracking()
            .Take(10).ToListAsync();

        if (produtos is null)
        {
            return NotFound("Produtos não encontrados...");
        }

        return produtos;
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public async Task<ActionResult<Produto>> Get(int id)
    {
        var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound($"Produto com id = {id} não encontrado...");
        }

        return produto;
    }

    [HttpPost]
    public ActionResult<Produto> Post(Produto produto)
    {
        if (produto is null)
            return BadRequest("Objeto produto é nulo");

        _context.Produtos.Add(produto);
        _context.SaveChanges();

        return new CreatedAtRouteResult("ObterProduto",
            new { id = produto.ProdutoId }, produto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Produto> Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return BadRequest($"O id = {id} informado não é igual ao id de produto");
        }

        _context.Entry(produto).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Produto> Delete(int id)
    {
        var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não localizado...");
        }

        _context.Produtos.Remove(produto);
        _context.SaveChanges();

        return Ok(produto);
    }
}
