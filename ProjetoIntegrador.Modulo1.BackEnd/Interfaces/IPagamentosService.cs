﻿using ProjetoIntegrador.Modulo1.BackEnd.Models;

namespace ProjetoIntegrador.Modulo1.BackEnd.Interfaces
{
    public interface IPagamentosService
    {
        Task<CorreiosResponse> CalcularFrete(PrecoPrazoRequest request);
        string GerarPixCopiaCola(double valor);
        Task<ResumoVenda> ObterResumoVenda(Guid guid);
        Task<Guid> RealizarVenda(Venda venda);
    }
}