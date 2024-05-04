using TangoSchool.DataAccess.DatabaseContexts.Interfaces;

namespace TangoSchool.DataAccess.Repositories.Interfaces;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    TEntity Add(TEntity entity);
    void AddRange(ICollection<TEntity> entities);
    void Update(TEntity entity);
    void Update(ICollection<TEntity> entities);

    IUnitOfWork UnitOfWork { get; }
}
