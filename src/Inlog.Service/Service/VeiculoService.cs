
using AutoMapper;
using Inlog.Domain.Dtos;
using Inlog.Domain.Entities;
using Inlog.Domain.Enum;
using Inlog.Domain.Interfaces.Repository;
using Inlog.Domain.Interfaces.Service;
using Inlog.Service.Interface;
using Inlog.Service.Validators;
using KissLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inlog.Service.Service
{
    public class VeiculoService : BaseService, IVeiculoService
    {
        private readonly IMapper _mapper;
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly INotificador _notificador;
        private readonly ILogger _logger;

        public VeiculoService(INotificador notificador, IMapper mapper, ILogger logger, IVeiculoRepository veiculoRepository) : base(notificador)
        {
            _mapper = mapper;
            _notificador = notificador;
            _logger = logger;
            _veiculoRepository = veiculoRepository;
        }

        public async Task<IEnumerable<VeiculoDetalheDto>> ObterTodos()
        {

            var result = _mapper.Map<IEnumerable<VeiculoDetalheDto>>(await _veiculoRepository.ObterTodos());

            var query = (from a in result
                         select new VeiculoDetalheDto()
                         {
                             Id = a.Id,
                             Chassi = a.Chassi,
                             Cor = a.Cor,
                             NumeroPassageiros = a.NumeroPassageiros,
                             TipoVeiculo = a.TipoVeiculo,
                             Descricao = a.TipoVeiculo == TipoVeiculo.Caminhao ? "Caminhão" : "Ônibus"

                         }).ToList();

            return query;
        }


        public async Task<VeiculoDetalheDto> ObterPorChassi(string chassi)
        {

            var result = _mapper.Map<VeiculoDetalheDto>(_veiculoRepository.Buscar(c => c.Chassi == chassi).Result.FirstOrDefault());

            if (result != null)
            {
                result.Descricao = result.TipoVeiculo == TipoVeiculo.Caminhao ? "Caminhão" : "Ônibus";

                return result;
            }
            else
            {

                Notificar("Veículo não encontrado no sistemas.");
                _logger.Info($"Veículo com chassi {chassi} não encontrado no sistemas.");
                return new VeiculoDetalheDto();
            }
           
        }

        public async Task<bool> Adicionar(VeiculoDto veiculoDto)
        {
            var veiculo = _mapper.Map<Veiculo>(veiculoDto);

            if (veiculo.TipoVeiculo == TipoVeiculo.Caminhao)
            {
                veiculo.NumeroPassageiros = 2;
            }
            else
            {
                veiculo.NumeroPassageiros = 42;
            }

            if (!ExecutarValidacao(new VeiculoValidation(), veiculo))
            {
                return false;
            }

            if (_veiculoRepository.Buscar(f => f.Chassi == veiculo.Chassi).Result.Any())
            {
                Notificar("Já existe um veículo cadastrado com este chassi informado.");
                _logger.Info($"Veículo com chassi {veiculo.Chassi} já cadastrado no sistemas.");
                return false;
            }
            veiculo.CreateAt = DateTime.UtcNow;
            await _veiculoRepository.Adicionar(veiculo);
            return true;
        }

        public async Task<bool> Atualizar( VeiculoDto veiculoDto)
        {
            var veiculo = _mapper.Map<Veiculo>(veiculoDto);

            if (veiculo.TipoVeiculo == TipoVeiculo.Caminhao)
            {
                veiculo.NumeroPassageiros = 2;
            }
            else
            {
                veiculo.NumeroPassageiros = 42;
            }

            if (!ExecutarValidacao(new VeiculoValidation(), veiculo))
            {
                return false;
            }

            var result = await _veiculoRepository.Buscar(f => f.Chassi == veiculoDto.Chassi);

            if (!result.Any())
            {
                Notificar("Não existe um veículo cadastrado com esse chassi.");
               _logger.Info($"Não existe um veículo cadastrado com o chassi { veiculoDto.Chassi} cadastrado no sistemas.");
                return false;
            }
            veiculo.Id = result.FirstOrDefault().Id;
            veiculo.UpdateAt = DateTime.UtcNow;
            await _veiculoRepository.Atualizar(veiculo);
            return true;

        }

        public async Task<VeiculoDto> Remover(string chassi)
        {

            var result = await _veiculoRepository.Buscar(c=>c.Chassi.Equals(chassi));

            if (result.Any())
            {

                await _veiculoRepository.Remover(result.FirstOrDefault().Id);
                var resultVeiculo = _mapper.Map<VeiculoDto>(result.FirstOrDefault());

                _logger.Info($"O veículo com o chassi {chassi} foi removido no sistemas.");
                return resultVeiculo;


            }
            else
                Notificar("Veículo não encontrado no sistemas.");     

            return null;
        }

        public void Dispose()
        {
            _veiculoRepository?.Dispose();

        }

    }
}
