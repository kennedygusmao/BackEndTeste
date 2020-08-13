using FluentValidation;
using Inlog.Domain.Entities;
using Inlog.Domain.Enum;
using Inlog.Service.Validations.Passageiro;

namespace Inlog.Service.Validators
{
    public class VeiculoValidation : AbstractValidator<Veiculo>
    {
        public VeiculoValidation()
        {
            RuleFor(f => f.Chassi)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 100)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(f => f.Cor)
               .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
               .Length(2, 100)
               .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");



            When(f => (int)f.TipoVeiculo == (int)TipoVeiculo.Caminhao, () =>
            {
                RuleFor(f => PassageiroValidacao.ValidarQuantidadePassageiroVeiculo(f.TipoVeiculo, f.NumeroPassageiros)).Equal(true)
                  .WithMessage("A quantidade de passageiro não coresponde ao veículo oni.");
            });


            When(f => (int)f.TipoVeiculo == (int)TipoVeiculo.Onibus, () =>
            {
                RuleFor(f => PassageiroValidacao.ValidarQuantidadePassageiroVeiculo(f.TipoVeiculo,f.NumeroPassageiros)).Equal(true)
                    .WithMessage("A quantidade de passageiro não coresponde ao veículo oni.");
            });

            RuleFor(f => f.TipoVeiculo).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(f => f.NumeroPassageiros).NotEmpty().WithMessage("O campo {PropertyName} precisa ter um valor");




        }
    }
}
