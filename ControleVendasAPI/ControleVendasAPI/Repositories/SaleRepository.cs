using ControleVendasAPI.Context;
using ControleVendasAPI.Models;
using ControleVendasAPI.Models.DTOS;
using ControleVendasAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleVendasAPI.Repositories;

public class SaleRepository : Repository<Sale>, ISalesRepository
{

    public SaleRepository(AppDbContext context) : base(context)
    {
    }

}