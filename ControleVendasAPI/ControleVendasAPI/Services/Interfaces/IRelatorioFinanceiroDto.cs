using ControleVendasAPI.DTOS;
using ControleVendasAPI.Models;

namespace ControleVendasAPI.Services.Interfaces;

public interface IRelatorioFinanceiroDto
{
    FinanceiroVendasDto RelatorioVendasDto(int saleId);
}