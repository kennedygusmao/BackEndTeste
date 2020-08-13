using Inlog.Domain.Enum;
using System;

namespace Inlog.Domain.Dtos
{
    public class VeiculoDetalheDto
    {
        public Guid Id { get; set; }
        public string Chassi { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }
        public string Descricao { get; set; }
        public byte NumeroPassageiros { get; set; }
        public string Cor { get; set; }
    }
}
