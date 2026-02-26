using ControleVendasAPI.DTOS;
using ControleVendasAPI.Models;
using ControleVendasAPI.Repositories.Interfaces;
using ControleVendasAPI.Services.Interfaces;
using ControleVendasAPI.StaticClasses;

namespace ControleVendasAPI.Services;

public class RelatorioFinanceiroDtoService : IRelatorioFinanceiroDto
{
    private readonly IUnitOfWork _uof;

    public RelatorioFinanceiroDtoService(IUnitOfWork uof)
    {
        _uof = uof;
    }


    public FinanceiroVendasDto RelatorioVendasDto(int saleId)
    {
        var sale = _uof.SalesRepository.GetById(b => b.Id == saleId);

        if (sale == null)
        {
            throw new ArgumentNullException("A venda nao existe");
        }
        

        var resultado = CalcularVendas(sale);


        return new FinanceiroVendasDto
        {
            Custo = (double)resultado.custo,
            Faturamento = (double)resultado.faturamento,
            Lucro = (double)resultado.lucro
        };
    }


    private (decimal faturamento, decimal custo, decimal lucro) CalcularVendas(Sale sale)
    {
        decimal faturamento = (decimal)(sale.Quantity * sale.SalePriceUnit);

        decimal custo = (decimal)(sale.Quantity * PriceConstants.KitConst);

        decimal lucro = (faturamento - custo);

        return  (faturamento, custo, lucro);
    }
}