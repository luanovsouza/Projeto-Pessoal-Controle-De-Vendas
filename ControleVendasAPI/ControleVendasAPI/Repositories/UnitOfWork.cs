using ControleVendasAPI.Context;
using ControleVendasAPI.Repositories.Interfaces;

namespace ControleVendasAPI.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public ISalesRepository SalesRepository { get; }
    public ISweetKitRepository SweetKitRepository { get; }
    public AppDbContext _context { get; }

    public UnitOfWork(ISalesRepository salesRepository, ISweetKitRepository sweetKitRepository, AppDbContext context)
    {
        SalesRepository = salesRepository;
        SweetKitRepository = sweetKitRepository;
        _context = context;
    }


    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }
}