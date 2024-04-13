using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IRepository<Categoria> _repository;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ILogger<CategoriasController> logger, ICategoriaRepository repository)
    {
        _repository = repository;
        _logger = logger;
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
        var categoria = _repository.GetAll();

        if (categoria is null)
            throw new ArgumentNullException(nameof(categoria));

        return Ok(categoria);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        var categoria = _repository.Get(c => c.CategoriaId == id);

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

        var categoriaCriada = _repository.Create(categoria);

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

        _repository.Update(categoria);

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Produto> Delete(int id)
    {
        var categoria = _repository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound($"Categoria com id = {id} não encontrada...");
        }

        var categoriaExcluida = _repository.Delete(categoria);

        return Ok(categoriaExcluida);
    }
}

Oi 