using TangoSchool.DataAccess.DatabaseContexts;
using TangoSchool.DataAccess.DatabaseContexts.Interfaces;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.DataAccess.Repositories;

internal class BaseRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    public IUnitOfWork UnitOfWork => Context;

    protected readonly TangoSchoolDbContext Context;

    protected BaseRepository(TangoSchoolDbContext context)
    {
        Context = context;
    }

    public virtual TEntity Add(TEntity entity)
    {
        return Context.Set<TEntity>().Add(entity).Entity;
    }

    public virtual void AddRange(ICollection<TEntity> entities)
    {
        Context.Set<TEntity>().AddRange(entities);
    }

    public virtual void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    public virtual void Update(ICollection<TEntity> entities)
    {
        Context.Set<TEntity>().UpdateRange(entities);
    }
}
