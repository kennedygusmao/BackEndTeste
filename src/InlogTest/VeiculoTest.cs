using AutoMapper;
using Inlog.Data.Context;
using Inlog.Data.Mapper;
using Inlog.Data.Repository;
using Inlog.Domain.Dtos;
using Inlog.Domain.Entities;
using Inlog.Domain.Interfaces.Repository;
using Inlog.Domain.Interfaces.Service;
using Inlog.Service.Interface;
using Inlog.Service.Notificacoes;
using Inlog.Service.Service;
using KissLog;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace InlogTest
{
    public class VeiculoTest
    {


        private readonly InlogDbContext _contextMemory;
        private readonly IVeiculoService _serviceMemory;
        private readonly IMapper mapperMemory;
        private readonly IVeiculoRepository _repositoryMemory;
        private readonly INotificador _notificadorMemory;
        private readonly Guid veiculo1 = Guid.NewGuid();
        private readonly Guid veiculo2 = Guid.NewGuid();
        private readonly Mock<ILogger> logger;


        public VeiculoTest()
        {
            logger = new Mock<ILogger>();

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<AutomapperConfig>();
            });
            mapperMemory = mapperConfig.CreateMapper();

            _contextMemory = InMemoryContextFactory.Create();

            this._notificadorMemory = new Notificador();

            this._repositoryMemory = new VeiculoRepository(_contextMemory);
            this._serviceMemory = new VeiculoService(_notificadorMemory, mapperMemory, logger.Object, _repositoryMemory);

            ConfigInMemory();
        }

        private void ConfigInMemory()
        {
            Veiculo veiculo;


            veiculo = new Veiculo()
            {
                Id = veiculo1,
                Chassi = "123",
                Cor = "branca",
                NumeroPassageiros = 2,
                TipoVeiculo = Inlog.Domain.Enum.TipoVeiculo.Caminhao

            };

            _contextMemory.Veiculos.Add(veiculo);

            veiculo = new Veiculo()
            {
                Id = veiculo2,
                Chassi = "12345",
                Cor = "preto",
                NumeroPassageiros = 42,
                TipoVeiculo = Inlog.Domain.Enum.TipoVeiculo.Onibus

            };

            _contextMemory.Veiculos.Add(veiculo);
            _contextMemory.SaveChanges();


            var entity1 = _contextMemory.Find<Veiculo>(veiculo1); //To Avoid tracking error
            _contextMemory.Entry(entity1).State = EntityState.Detached;
            var entity2 = _contextMemory.Find<Veiculo>(veiculo2); //To Avoid tracking error
            _contextMemory.Entry(entity2).State = EntityState.Detached;


        }

        private bool OperacaoValida()
        {
            return !_notificadorMemory.TemNotificacao();
        }

        [Fact]
        public void ObterVeiculoPeloChassi()
        {        

            var chassi = "123";
            var result = _serviceMemory.ObterPorChassi(chassi).Result;
            var operacaoIsValid = OperacaoValida();       
                       
            Assert.NotNull(result);
            Assert.True(operacaoIsValid);
            Assert.Equal(chassi, result.Chassi);

        }

        [Fact]
        public void ObterTodosVeiculo()
        {

            var quantidadeVeiculos = _contextMemory.Veiculos.Count();
            var result = _serviceMemory.ObterTodos().Result;
            var operacaoIsValid = OperacaoValida();


            Assert.NotNull(result);
            Assert.True(operacaoIsValid);
            Assert.Equal(quantidadeVeiculos, result.Count());

        }

        [Fact]
        public void DeveCadastrarUmOnibus()
        {


            var chassi = Guid.NewGuid();
            var quantidadePassgeiro = 42;

            var veiculo = new VeiculoDto()
            {
                Chassi = chassi.ToString(),
                Cor = "azul",
                TipoVeiculo = Inlog.Domain.Enum.TipoVeiculo.Onibus

            };


            var result = _serviceMemory.Adicionar(veiculo).Result;
            var operacaoIsValid = OperacaoValida();
            var notificacao = _notificadorMemory.ObterNotificacoes();

            var veiculoCadastrado = _serviceMemory.ObterPorChassi(chassi.ToString()).Result;

            Assert.NotNull(veiculoCadastrado);
            Assert.True(operacaoIsValid);
            Assert.Equal(chassi.ToString(), veiculoCadastrado.Chassi.ToString());
            Assert.Equal(quantidadePassgeiro, veiculoCadastrado.NumeroPassageiros);

        }

        [Fact]
        public void DeveCadastrarUmCaminha()
        {


            var chassi = Guid.NewGuid();
            var quantidadePassgeiro = 2;

            var veiculo = new VeiculoDto()
            {
                Chassi = chassi.ToString(),
                Cor = "amarelo",
                TipoVeiculo = Inlog.Domain.Enum.TipoVeiculo.Caminhao

            };


            var result = _serviceMemory.Adicionar(veiculo).Result;
            var operacaoIsValid = OperacaoValida();
            var notificacao = _notificadorMemory.ObterNotificacoes();

            var veiculoCadastrado = _serviceMemory.ObterPorChassi(chassi.ToString()).Result;

            Assert.NotNull(veiculoCadastrado);
            Assert.True(operacaoIsValid);
            Assert.Equal(chassi.ToString(), veiculoCadastrado.Chassi.ToString());
            Assert.Equal(quantidadePassgeiro, veiculoCadastrado.NumeroPassageiros);

        }


        [Fact]
        public void DeveAlterarUmVeiculo()
        {
            var chassi = "12345";
            var cor = "dourado";

            var veiculoCadastrado = _serviceMemory.ObterPorChassi(chassi.ToString()).Result;
            veiculoCadastrado.Cor = cor;

            var result = _serviceMemory.Atualizar(mapperMemory.Map<VeiculoDto>(veiculoCadastrado)).Result;

            var veiculoAlterado = _serviceMemory.ObterPorChassi(chassi.ToString()).Result;


            Assert.NotNull(veiculoAlterado);
            Assert.Equal(chassi.ToString(), veiculoCadastrado.Chassi.ToString());
            Assert.Equal(cor, veiculoCadastrado.Cor);

        }


        [Fact]
        public void DeveExcluirUmVeiculo()
        {
            var chassi = "12345";

            var veiculoCadastrado = _serviceMemory.ObterPorChassi(chassi.ToString()).Result;
            var result = _serviceMemory.Remover(veiculoCadastrado.Chassi).Result;
            var veiculos = _serviceMemory.ObterTodos().Result;

            var veiculodeletadp = veiculos.FirstOrDefault(c => c.Chassi == chassi);

            Assert.Null(veiculodeletadp);

        }

        [Fact]
        public void DeveValidarVeiculoNaoEncontrado()
        {
            var chassi = "34234324";
            var mensagem = "Veículo não encontrado no sistemas.";

            var veiculoCadastrado = _serviceMemory.ObterPorChassi(chassi.ToString()).Result;
            var notificacao = _notificadorMemory.ObterNotificacoes();
            var mensagemValidacao = notificacao.FirstOrDefault(c => c.Mensagem == mensagem);

            Assert.Equal(mensagem, mensagemValidacao.Mensagem);

        }


    }
}
