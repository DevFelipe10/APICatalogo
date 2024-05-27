using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApiCatalogoxUnitTests.UnitTests.Produtos;

public class ProdutosUnitTestController
{
    public IUnitOfWork repository;
    public IMapper mapper;

    public static DbContextOptions<AppDbContext> dbContextOptions { get; }

    public static string connectionString = WebApplication
                                            .CreateBuilder()
                                            .Configuration
                                            .GetConnectionString("DefaultConnection")!;

    static ProdutosUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            .Options;
    }

    public ProdutosUnitTestController()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ProdutoDTOMappingProfile());
        });

        mapper = config.CreateMapper();

        var context = new AppDbContext(dbContextOptions);
        repository = new UnitOfWork(context);
    }
}
