
using Inlog.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inlog.Domain.Interfaces.Service
{
    public interface IVeiculoService : IDisposable
    {
        Task<IEnumerable<VeiculoDetalheDto>> ObterTodos();
        Task<VeiculoDetalheDto> ObterPorChassi(string chassi);
        Task<bool> Adicionar(VeiculoDto veiculoDto);
        Task<bool> Atualizar(VeiculoDto veiculoDto);
        Task<VeiculoDto> Remover(string chassi);
    }
}
