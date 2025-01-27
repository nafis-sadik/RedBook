using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using CaseExtensions;
using System.Text;
using Microsoft.Data.SqlClient;

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

        //public async Task BulkInsertAsync(IEnumerable<TEntity> entities) => await _dbSet.AddRangeAsync(entities);
        public async Task<IEnumerable<TEntity>> BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentException("No valid entities found!");

            Type entityType = typeof(TEntity);
            string? tableName = _dbContext.Model.FindEntityType(entityType)?.GetTableName();
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException("Table name not found!");

            PropertyInfo? primaryKeyColumnInfo = _dbContext.Model.FindEntityType(typeof(TEntity))?.FindPrimaryKey()?.Properties.Single().PropertyInfo;
            if (primaryKeyColumnInfo == null)
                throw new ArgumentException($"Unable to locate primary key for type {nameof(entityType)}");

            Type primaryKeyType = primaryKeyColumnInfo.PropertyType;

            List<PropertyInfo> properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !p.GetGetMethod().IsVirtual && p.Name != primaryKeyColumnInfo.Name)
                .ToList();

            string columns = string.Join(", ", properties.Select(p => p.Name));
            StringBuilder values = new StringBuilder();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int parameterIndex = 0;

            foreach (var entity in entities)
            {
                var valueList = new List<string>();
                for (int i = 0; i < properties.Count; i++)
                {
                    var property = properties[i];
                    var parameterName = $"@p{parameterIndex++}";
                    var value = property.GetValue(entity);
                    valueList.Add(parameterName);
                    parameters.Add(new SqlParameter(parameterName, value ?? DBNull.Value));
                }

                values.AppendLine($"({string.Join(", ", valueList)}),");
            }

            string insertSql = $@"
                INSERT INTO {tableName} ({columns})
                OUTPUT INSERTED.{primaryKeyColumnInfo.Name}
                VALUES {values.ToString().TrimEnd(',', '\r', '\n')};";

            var insertedIds = new List<object>();
            await using (var connection = _dbContext.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = insertSql;
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    var entityList = entities.ToList();
                    int index = 0;
                    await using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            insertedIds.Add(Convert.ChangeType(reader[0], primaryKeyType));
                            entityType.GetProperty(primaryKeyColumnInfo.Name)?.SetValue(entityList[index], insertedIds.Last());
                            index++;
                        }
                    }
                }
            }

            // Attach the inserted entities back to the context to ensure they are tracked
            foreach (var entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Unchanged;
            }

            return entities;
        }

        private string FormatValueForSql(object value){
            if (value == null) return "NULL"; 
            if (value is string || value is DateTime) return $"'{value.ToString()?.Replace("'", "''")}'"; 
            if (value is bool) return (bool)value ? "1" : "0"; 
            if (Nullable.GetUnderlyingType(value.GetType()) != null) return value?.ToString(); 
            return value.ToString();
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
                    {
                        // Adjust based on your needs
                        string elementValue = element.ToString();
                        if (elementValue.Contains("."))
                            return element.GetDecimal();
                        else
                            return element.GetInt32();
                    }
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
            foreach (KeyValuePair<string, object> property in keyValuePairs)
            {
                string propertyName = property.Key.ToPascalCase();
                PropertyEntry? targetProperty = _dbContext.Entry(entity).Properties.FirstOrDefault(prop => prop.Metadata.Name == propertyName);
                if (targetProperty == null) continue;
                PropertyInfo? targetPropertyInfo = targetProperty.Metadata.PropertyInfo;
                if (targetPropertyInfo == null) throw new ArgumentException($"Unable to locate property {property.Key} in type {typeof(TEntity)}");

                if (targetPropertyInfo.Name != primaryKeyColumnInfo.Name)
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
