﻿using RedBook.Core.EntityFramework;
using RedBook.Core.Models;
using System.Linq.Expressions;

namespace RedBook.Core.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        IQueryable<TEntity> TrackableQuery();
        IQueryable<TEntity> UnTrackableQuery();

        // Create Async
        Task<TEntity> InsertAsync(TEntity entity);

        // Read
        TEntity? Get(int id);
        TEntity? Get(string id);
        TEntity? Get(object id);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where);

        // Read Async
        Task<TEntity?> GetAsync(int id);
        Task<TEntity?> GetAsync(string id);
        Task<TEntity?> GetAsync(object id);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> where);

        // Update
        TEntity Update(TEntity entity);
        void Patch(object pk, IDictionary<string, object> newEntries);

        // Delete
        Task DeleteAsync(int id);
        Task DeleteAsync(string id);
        void Delete(TEntity entity);
        Task DeleteAsync(Expression<Func<TEntity, bool>> where);

        // Utilities
        Task SaveChangesAsync();

         void DetachAllEntities();
    }
}
