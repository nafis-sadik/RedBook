using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;

namespace RedBook.Core.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> TrackableQuery() => _dbSet.AsTracking().AsQueryable();
        public virtual IQueryable<TEntity> UnTrackableQuery() => _dbSet.AsNoTracking().AsQueryable();

        // Create
        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = await _dbSet.AddAsync(entity);
            if (entityEntry.State == EntityState.Added) return entityEntry.Entity;
            else throw new InvalidOperationException($"Failed to insert {entity}");
        }

        // Read Async
        public TEntity? Get(int id) => _dbSet.Find(id);
        public TEntity? Get(string id) => _dbSet.Find(id);
        public TEntity? Get(object id) => _dbSet.Find(id);
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> query) => UnTrackableQuery().Where(query).ToList();

        // Read
        public async Task<TEntity?> GetAsync(int id) => await _dbSet.FindAsync(id);
        public async Task<TEntity?> GetAsync(string id) => await _dbSet.FindAsync(id);
        public async Task<TEntity?> GetAsync(object id) => await _dbSet.FindAsync(id);
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> query) => await UnTrackableQuery().Where(query).ToListAsync();

        // Update
        public virtual TEntity Update(TEntity entity) => _dbSet.Update(entity).Entity;
        public void Patch(object pk, IDictionary<string, object> newEntries)
        {
            TEntity? entity = Activator.CreateInstance(typeof(TEntity)) as TEntity;
            if(entity == null) throw new ArgumentNullException($"Unable to create entity of type {nameof(entity)}");

            PropertyInfo? primaryKeyColumn = _dbContext.Model.FindEntityType(typeof(TEntity))?.FindPrimaryKey()?.Properties.Single().PropertyInfo;
            if (primaryKeyColumn == null) throw new ArgumentException($"Unable to locate primary key for type {nameof(entity)}");

            if (primaryKeyColumn.PropertyType == pk.GetType())
                primaryKeyColumn.SetValue(entity, pk);
            else
                throw new ArgumentException($"Cannot set value of type {pk.GetType()} for the primary key {primaryKeyColumn.Name} of type {primaryKeyColumn.PropertyType}");
            
            foreach (KeyValuePair<string, object> property in newEntries)
            {
                PropertyEntry targetProperty = _dbContext.Entry(entity).Property(property.Key);
                if (targetProperty.GetType() == property.Value.GetType())
                {
                    targetProperty.CurrentValue = property.Value;
                    targetProperty.IsModified = true;
                }
                else
                    throw new ArgumentException($"The type of the provided value does not match the type of the property. Property: {property.Key}, Provided Type: {property.Value.GetType()}, Expected Type: {targetProperty.CurrentValue?.GetType()}");
            }
        }

        // Delete
        public void Delete(TEntity entity) => _dbSet.Remove(entity);

        public async Task DeleteAsync(int id)
        {
            var targetRow = await GetAsync(id);
            if (targetRow != null)
                _dbSet.Remove(targetRow);
            else
                throw new ArgumentException($"Object with identifier {id} was not found");
        }

        public async Task DeleteAsync(string id)
        {
            var targetRow = await GetAsync(id);
            if (targetRow != null)
                _dbSet.Remove(targetRow);
            else
                throw new ArgumentException($"Object with identifier {id} was not found");
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> where)
        {
            var objects = await UnTrackableQuery().Where(where).ToArrayAsync();
            foreach (var obj in objects)
            {
                _dbSet.Remove(obj);
            }
        }

        // Utilities
        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public async Task Dispose() => await _dbContext.DisposeAsync();

        public void DetachAllEntities()
        {
            IEnumerable<EntityEntry> changedEntriesCopy = _dbContext.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified
                        || x.State == EntityState.Added
                        || x.State == EntityState.Deleted);
            foreach (var entity in changedEntriesCopy)
            {
                entity.State = EntityState.Detached;
            }
        }
    }
}
