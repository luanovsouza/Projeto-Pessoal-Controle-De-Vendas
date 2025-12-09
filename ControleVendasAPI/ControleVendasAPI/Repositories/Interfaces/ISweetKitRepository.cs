using ControleVendasAPI.Models;

namespace ControleVendasAPI.Repositories.Interfaces;

public interface ISweetKitRepository
{
    Task<IEnumerable<SweetKit>> GetSweetKits();
    
    Task<SweetKit> GetKitsById(int id);
    
    Task<SweetKit> PostKits(SweetKit sweetKit);
    
    Task<SweetKit> PutKit(int id, SweetKit sweetKit);
    
    Task<SweetKit> DeleteKit(int id);
}