using ControleVendasAPI.Context;

namespace ControleVendasAPI.Repositories.Interfaces;

public interface IUnitOfWork
{
    ISalesRepository SalesRepository { get; }
    ISweetKitRepository SweetKitRepository { get; }
    Task Commit();
}