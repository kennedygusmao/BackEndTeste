
using Inlog.Domain.Enum;


namespace Inlog.Domain.Entities
{
    public class Veiculo : EntityBase
    {
        public string Chassi { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }
        public byte NumeroPassageiros { get; set; }
        public string Cor { get; set; }
    }
}
