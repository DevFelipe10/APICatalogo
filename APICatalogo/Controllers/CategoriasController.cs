using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProduto()
    {
        _logger.LogInformation("==========Get api/categorias/produtos================");

        return await _context.Categorias
        .Include(p => p.Produtos)
        .AsNoTracking()
        .Where(c => c.CategoriaId <= 5).ToListAsync();
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public async Task<ActionResult<IEnumerable<Categoria>>> Get()
    {
        return await _context.Categorias.AsNoTracking().Take(10).ToListAsync();
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<Categoria>> Get(int id)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound($"Produto com id = {id} não encontrado...");
        }

        return categoria;
    }

    [HttpPost]
    public ActionResult<Categoria> Post(Categoria categoria)
    {
        if (categoria is null)
        {
            return BadRequest("Objeto categoria é nulo");
        }

        _context.Categorias.Add(categoria);
        _context.SaveChanges();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Categoria> Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
        {
            return BadRequest($"O id = {id} informado não é igual ao objeto categoria");
        }

        _context.Entry(categoria).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Produto> Delete(int id)
    {
        var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound($"Categoria com id = {id} não encontrada...");
        }

        return Ok(categoria);
    }
}