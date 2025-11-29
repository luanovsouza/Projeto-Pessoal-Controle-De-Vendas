using ControleVendasAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleVendasAPI.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Cost> Costs { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SweetKit> SweetKits { get; set; }
}