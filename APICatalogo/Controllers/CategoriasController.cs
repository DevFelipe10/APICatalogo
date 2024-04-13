using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
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
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        var categorias = _uof.CategoriaRepository.GetAll();

        if (categorias is null)
            return NotFound("Não existem categorias...");

        var categoriasDto = categorias.ToCategoriaDTOList();

        return Ok(categoriasDto);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
        var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id = {id} não encontrada...");
            return NotFound($"Produto com id = {id} não encontrado...");
        }

        var categoriaDto = categoria.ToCategoriaDto();

        return Ok(categoriaDto);
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
        {
            return BadRequest("Objeto categoria é nulo");
        }

        var categoria = categoriaDto.ToCategoria();

        var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
        _uof.Commit();

        var novaCategoriaDto = categoriaCriada.ToCategoriaDto();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
        {
            return BadRequest($"O id = {id} informado não é igual ao objeto categoria");
        }

        var categoria = categoriaDto.ToCategoria();

        var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
        _uof.Commit();

        var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDto();

        return Ok(categoriaAtualizadaDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound($"Categoria com id = {id} não encontrada...");
        }

        var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
        _uof.Commit();

        var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDto();

        return Ok(categoriaExcluidaDto);
    }
}
