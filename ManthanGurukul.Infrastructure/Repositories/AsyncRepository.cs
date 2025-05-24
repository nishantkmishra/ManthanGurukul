using ManthanGurukul.Application.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ManthanGurukul.Infrastructure.Data;

namespace ManthanGurukul.Infrastructure.Repositories
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : class
    {
        private readonly ManthanGurukulDBContext _db;
        internal DbSet<T> DbSet { get; private set; }

        public AsyncRepository(ManthanGurukulDBContext db)
        {
            this._db = db;
            this.DbSet = _db.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await this.DbSet.AddAsync(entity);
            await this._db.SaveChangesAsync();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IQueryable<T> query = DbSet;
            return await query.ToListAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = DbSet;
            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task UpdateAsync(T entity)
        {
            // Check if an entity with the same key is already being tracked
            var keyProperties = typeof(T).GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(KeyAttribute)) ||
                            Attribute.IsDefined(p, typeof(DatabaseGeneratedAttribute)))
                .ToList();

            // Build the key values to identify the entity
            var keyValues = keyProperties.Select(p => p.GetValue(entity)).ToArray();

            var trackedEntity = this._db.ChangeTracker.Entries<T>()
                .FirstOrDefault(e => keyProperties.All(kp =>
                    kp.GetValue(e.Entity).Equals(kp.GetValue(entity))));

            if (trackedEntity != null)
            {
                // Update the tracked entity with the new values
                this._db.Entry(trackedEntity.Entity).CurrentValues.SetValues(entity);
            }
            else
            {
                // Attach the entity to the context if it's not already tracked
                var existingEntity = await this.DbSet.FindAsync(keyValues);
                if (existingEntity != null)
                {
                    this._db.Entry(existingEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    // If no existing entity is found, attach the new entity
                    this.DbSet.Attach(entity);
                }
            }

            // Prevent modification of primary key or identity properties
            var entry = this._db.Entry(entity);
            foreach (var keyProperty in keyProperties)
            {
                entry.Property(keyProperty.Name).IsModified = false;
            }

            // Save changes
            await this._db.SaveChangesAsync();
        }

        public async Task<T> GetLastOrDefaultAsync(Expression<Func<T, bool>> filter, Expression<Func<T, object>> orderBy)
        {
            IQueryable<T> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.OrderByDescending(orderBy).FirstOrDefaultAsync();
        }

    }
}
