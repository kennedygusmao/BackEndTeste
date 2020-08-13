using Inlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inlog.Data.EntityConfig
{
    public class VeiculoConfiguration : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(c => c.Chassi)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(c => c.TipoVeiculo)
                .IsRequired();


            builder.Property(c => c.NumeroPassageiros)
                .IsRequired();


            builder.Property(c => c.CreateAt)
                .HasColumnName("DataCadastro");

            builder.Property(c => c.UpdateAt)
               .HasColumnName("DataAtualizacao");          

         

            builder.ToTable("Veiculos");
        }
    }
}
