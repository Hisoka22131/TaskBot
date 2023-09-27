using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TaskBot.Library.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskBot.Repository.GenericRepo;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
{
    private readonly DbContext _context;
    private DbSet<TEntity> _dbSet => _context.Set<TEntity>();

    protected GenericRepository(DbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public virtual async Task Insert(TEntity entity) => await _dbSet.AddAsync(entity);

    public virtual async Task Insert(params TEntity[] entities) => await _dbSet.AddRangeAsync(entities);

    public virtual async Task Insert(IEnumerable<TEntity> entities) => await _dbSet.AddRangeAsync(entities);

    public virtual async Task Remove(int? id)
    {
        var typeInfo = typeof(TEntity).GetTypeInfo();
        var key = _context.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
        var property = typeInfo.GetProperty(key?.Name);
        if (property != null)
        {
            var entity = Activator.CreateInstance<TEntity>();
            property.SetValue(entity, id);
            _context.Entry(entity).State = EntityState.Deleted;
        }
        else
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                Remove(entity);
            }
        }
    }

    public virtual async Task Remove(TEntity entity) => _dbSet.Remove(entity);

    public virtual async Task Remove(params TEntity[] entities) => _dbSet.RemoveRange(entities);

    public virtual async Task Remove(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

    public virtual async Task Update(TEntity entity) => _dbSet.Update(entity);

    public virtual async Task Update(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

    public virtual async Task Update(params TEntity[] entities) => _dbSet.UpdateRange(entities);

    public virtual async Task InsertOrUpdate(TEntity entity)
    {
        if (_dbSet.Any(_ => _.Id == entity.Id)) Update(entity);
        else
            Insert(entity);
    }

    public virtual async Task InsertOrUpdate(params TEntity[] entities)
    {
        foreach (var entity in entities)
        {
            if (_dbSet.Any(_ => _.Id == entity.Id)) Update(entity);
            else Insert(entity);
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetEntities() => _dbSet.ToList();

    public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate) => await _dbSet.AnyAsync(predicate);
    
    public async Task<IEnumerable<TEntity>> GetEntities(params Expression<Func<TEntity, object>>[] includes)
        => includes.Aggregate(_dbSet.Where(q => true), (current, includeProperty) => current.Include(includeProperty));


    protected async Task<TEntity> GetEntity(int? id) => await _context.FindAsync<TEntity>(id);

    protected TEntity GetEntity(Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, object>>[] includes) =>
        includes.Aggregate(_dbSet.Where(predicate), (current, includeProperty) => current.Include(includeProperty))
            .FirstOrDefault();
}