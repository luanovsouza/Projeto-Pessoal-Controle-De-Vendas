using ControleVendasAPI.Context;
using ControleVendasAPI.Models;
using ControleVendasAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleVendasAPI.Repositories;

public class SweetKitRepository : Repository<SweetKit>, ISweetKitRepository
{
    public SweetKitRepository(AppDbContext context) : base(context){}

}