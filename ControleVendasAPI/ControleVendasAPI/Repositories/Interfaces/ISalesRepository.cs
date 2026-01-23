using ControleVendasAPI.Models;
using ControleVendasAPI.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace ControleVendasAPI.Repositories.Interfaces;

public interface ISalesRepository :  IRepository<Sale>
{
  PagedList<Sale> GetSales(SalesParameters saleParameters);
}