using Inlog.Data.Context;
using Inlog.Domain.Entities;
using Inlog.Domain.Interfaces.Repository;


namespace Inlog.Data.Repository
{
    public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(InlogDbContext context) : base(context)
        {
        }

    }
}
