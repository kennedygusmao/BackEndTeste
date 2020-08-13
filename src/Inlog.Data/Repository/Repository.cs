using Inlog.Data.Context;
using Inlog.Domain.Entities;
using Inlog.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Inlog.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase, new()
    {
        protected readonly InlogDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(InlogDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id).ConfigureAwait(false);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges().ConfigureAwait(false);
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges().ConfigureAwait(false);
        }

        public virtual async Task Remover(Guid id)
        {
            DbSet.Remove(new TEntity { Id = id });
            await SaveChanges().ConfigureAwait(false);
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}
