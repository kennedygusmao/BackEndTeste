using Inlog.API.Controllers;
using Inlog.Domain.Dtos;
using Inlog.Domain.Interfaces.Service;
using Inlog.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inlog.API.V1.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/veiculos")]
    public class VeiculosController : MainController
    {

        private readonly IVeiculoService _veiculoService;
        public VeiculosController(INotificador notificador, IVeiculoService veiculoService) : base(notificador)
        {
            _veiculoService = veiculoService;
        }

        /// <summary>
        /// Consulta todos os veículos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VeiculoDetalheDto>>> ObterTodos()
        {

            return CustomResponse(await _veiculoService.ObterTodos());
        }

        /// <summary>
        /// Obter veículo pelo número do chassi
        /// </summary>
        /// <param name="chassi"></param>
        /// <returns></returns>
        [HttpGet("{chassi}")]
        public async Task<ActionResult<VeiculoDetalheDto>> ObterPorChassi(string chassi)
        {
            return CustomResponse(await _veiculoService.ObterPorChassi(chassi));
        }



        /// <summary>
        /// Cadastrar Veículo
        /// </summary>
        /// <param name="veiculoDto"></param>
        /// <remarks>
        /// <para>Informar no campo:</para>
        /// <para>  TipoVeiculo = 1  para Caminhão  2 Ônibus</para>   
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<VeiculoDto>> Adicionar([FromForm] VeiculoDto veiculoDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            await _veiculoService.Adicionar(veiculoDto);

            return CustomResponse(veiculoDto);
        }

        /// <summary>
        /// Atualizar o Veículo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="veiculoDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<VeiculoDto>> Atualizar([FromForm] string chassi, [FromForm] VeiculoDetalheDto veiculoDto)
        {

            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            await _veiculoService.Atualizar(chassi, veiculoDto);

            return CustomResponse(veiculoDto);
        }

        /// <summary>
        /// Excluir veículo peloid
        /// </summary>
        /// <param name="chassi"></param>
        /// <returns></returns>
        [HttpDelete("{chassi}")]
        public async Task<ActionResult<VeiculoDto>> Excluir(string chassi)
        {
            var veiculo = await _veiculoService.Remover(chassi);
            return CustomResponse(veiculo);
        }
    }
}