using ControleVendasAPI.Context;
using ControleVendasAPI.Models;
using ControleVendasAPI.Pagination;
using ControleVendasAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleVendasAPI.Repositories;

public class SaleRepository : Repository<Sale>, ISalesRepository
{

    public SaleRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Sale> GetSales(SalesParameters saleParameters)
    {
        var sales = GetAll().OrderBy(sl => sl.Id).AsQueryable();
        var salesOrdenados = PagedList<Sale>.ToPagedList(sales, saleParameters.pageNumber,  saleParameters.PageSize);
        return salesOrdenados;
    }
}