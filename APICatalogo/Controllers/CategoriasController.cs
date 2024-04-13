using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(IUnitOfWork uof,
        ILogger<CategoriasController> logger)
    {
        _logger = logger;
        _uof = uof;
    }

    //[HttpGet("produtos")]
    //public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProduto()
    //{
    //    _logger.LogInformation("==========Get api/categorias/produtos================");

    //    return await _context.Categorias
    //    .Include(p => p.Produtos)
    //    .AsNoTracking()
    //    .Where(c => c.CategoriaId <= 5).ToListAsync();
    //}

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        var categoria = _uof.CategoriaRepository.GetAll();

        if (categoria is null)
            throw new ArgumentNullException(nameof(categoria));

        return Ok(categoria);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id = {id} não encontrada...");
            return NotFound($"Produto com id = {id} não encontrado...");
        }

        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult<Categoria> Post(Categoria categoria)
    {
        if (categoria is null)
        {
            return BadRequest("Objeto categoria é nulo");
        }

        var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
        _uof.Commit();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoriaCriada.CategoriaId }, categoriaCriada);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Categoria> Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
        {
            return BadRequest($"O id = {id} informado não é igual ao objeto categoria");
        }

        _uof.CategoriaRepository.Update(categoria);
        _uof.Commit();

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Produto> Delete(int id)
    {
        var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound($"Categoria com id = {id} não encontrada...");
        }

        var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
        _uof.Commit();

        return Ok(categoriaExcluida);
    }
}
