using System.Linq.Expressions;

namespace TaskBot.Repository.GenericRepo;

public interface IGenericRepository<TEntity> where TEntity : class
{
    public Task Insert(params TEntity[] entities);
    public Task Insert(IEnumerable<TEntity> entities);
    public Task Remove(int? id);
    public Task Remove(TEntity entity);
    public Task Remove(params TEntity[] entities);
    public Task Remove(IEnumerable<TEntity> entities);
    public Task Update(TEntity entity);
    public Task Update(IEnumerable<TEntity> entities);
    public Task Update(params TEntity[] entities);
    public Task InsertOrUpdate(TEntity entity);
    public Task InsertOrUpdate(params TEntity[] entities);
    Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
    public Task<IEnumerable<TEntity>> GetEntities();
    public Task<IEnumerable<TEntity>> GetEntities(params Expression<Func<TEntity, object>>[] includes);
}