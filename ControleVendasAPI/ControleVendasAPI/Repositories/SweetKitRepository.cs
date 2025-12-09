using ControleVendasAPI.Context;
using ControleVendasAPI.Models;
using ControleVendasAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControleVendasAPI.Repositories;

public class SweetKitRepository : ISweetKitRepository
{
    private readonly AppDbContext _context;

    public SweetKitRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SweetKit>> GetSweetKits()
    {
        var kits = await _context.SweetKits.ToListAsync();
        
        if(kits ==  null)
            throw new ArgumentNullException(nameof(kits));
        
        return kits;
    }

    public async Task<SweetKit> GetKitsById(int id)
    {
        var sweetKit = await _context.SweetKits.FirstOrDefaultAsync(s => s.Id == id);
        
        if(sweetKit == null)
            throw new ArgumentNullException(nameof(sweetKit));
        
        return sweetKit;
    }

    public async Task<SweetKit> PostKits(SweetKit sweetKit)
    {
        if (sweetKit == null)
            throw new ArgumentNullException(nameof(sweetKit));
        
        _context.Add(sweetKit);
        await _context.SaveChangesAsync();

        return sweetKit;
    }

    public async Task<SweetKit> PutKit(int id, SweetKit sweetKit)
    {
        var kit = _context.SweetKits.FirstOrDefault(s => s.Id == id);
        
        if (kit == null)
            throw new ArgumentNullException(nameof(sweetKit));
        
        if(sweetKit == null)
            throw new ArgumentNullException(nameof(sweetKit));
        
        _context.Entry(kit).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return sweetKit;
    }

    public async Task<SweetKit> DeleteKit(int id)
    {
        var kitDeleted = _context.SweetKits.FirstOrDefault(s => s.Id == id);
        
        if (kitDeleted == null)
            throw new ArgumentNullException(nameof(id));
        
        _context.Remove(kitDeleted);
        await _context.SaveChangesAsync();
        
        return kitDeleted;
    }
}