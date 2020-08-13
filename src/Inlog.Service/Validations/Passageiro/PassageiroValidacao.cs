using Inlog.Domain.Enum;

namespace Inlog.Service.Validations.Passageiro
{
    public static class PassageiroValidacao
    {
        public const byte QuantidadePassageiroCaminhao = 2;
        public const byte QuantidadePassageiroOnibus = 42;


        public static bool ValidarQuantidadePassageiroVeiculo(TipoVeiculo tipo, byte quantidade)
        {
            if (TipoVeiculo.Caminhao == tipo &&  quantidade == QuantidadePassageiroCaminhao)
                return true;

            else if (TipoVeiculo.Onibus == tipo && quantidade == QuantidadePassageiroOnibus)
                return true;



            return false;
        }
    }
}
