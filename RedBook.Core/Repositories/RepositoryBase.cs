using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

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

        // Helper method
        private object ParseJsonElement(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.String:
                    return element.GetString();
                case JsonValueKind.Number:
                    return element.GetDecimal(); // Adjust based on your needs
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Object:
                    var nestedDictionary = new Dictionary<string, object>();
                    foreach (var nestedProperty in element.EnumerateObject())
                    {
                        nestedDictionary[nestedProperty.Name] = ParseJsonElement(nestedProperty.Value);
                    }
                    return nestedDictionary;
                default:
                    return null; // Handle other value kinds as needed
            }
        }
        // Update
        public virtual TEntity Update(TEntity entity) => _dbSet.Update(entity).Entity;

        public void ColumnUpdate(object pk, Dictionary<string, object> keyValuePairs)
        {
            // Generate entity object
            TEntity? entity = Activator.CreateInstance(typeof(TEntity)) as TEntity;
            if (entity == null) throw new ArgumentNullException($"Unable to create entity of type {nameof(entity)}");

            // Get primary key column
            PropertyInfo? primaryKeyColumnInfo = _dbContext.Model.FindEntityType(typeof(TEntity))?.FindPrimaryKey()?.Properties.Single().PropertyInfo;
            if (primaryKeyColumnInfo == null) throw new ArgumentException($"Unable to locate primary key for type {nameof(entity)}");

            // Set primary key value
            if (primaryKeyColumnInfo.PropertyType == pk.GetType())
                primaryKeyColumnInfo.SetValue(entity, pk);
            else
                throw new ArgumentException($"Cannot set value of type {pk.GetType()} for the primary key {primaryKeyColumnInfo.Name} of type {primaryKeyColumnInfo.PropertyType}");

            // Update entity entries and mark as modified for update operation
            foreach (var property in keyValuePairs)
            {
                PropertyEntry targetProperty = _dbContext.Entry(entity).Property(property.Key);
                PropertyInfo? targetPropertyInfo = targetProperty.Metadata.PropertyInfo;
                if (targetPropertyInfo == null) throw new ArgumentException($"Unable to locate property {property.Key} in type {typeof(TEntity)}");

                if (targetPropertyInfo.Name == property.Key && targetPropertyInfo.Name != primaryKeyColumnInfo.Name)
                {
                    if (targetPropertyInfo.PropertyType.IsAssignableFrom(property.Value?.GetType()))
                        targetProperty.CurrentValue = property.Value;
                    else if (property.Value != null && property.Value.GetType() == typeof(JsonElement))
                        targetProperty.CurrentValue = ParseJsonElement((JsonElement)property.Value);
                    else
                        throw new ArgumentException($"The type of the provided value does not match the type of the property. Property: {property.Key.ToString()}, Provided Type: {property.GetType().ToString()}, Expected Type: {targetProperty.CurrentValue?.GetType()}");

                    targetProperty.IsModified = true;
                }
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

        public async Task<int> DeleteAsync(string propertyName, string symbol, string value)
        {
            // Get the entity type and its SQL table name
            IEntityType? entityType = _dbContext.Model.FindEntityType(typeof(TEntity));
            var tableName = entityType?.GetTableName();

            // Build the SQL command
            string sqlCommand = $"DELETE FROM {tableName} WHERE {propertyName} {symbol} {value}";

            // Execute the SQL command
            return await _dbContext.Database.ExecuteSqlRawAsync(sqlCommand);
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
