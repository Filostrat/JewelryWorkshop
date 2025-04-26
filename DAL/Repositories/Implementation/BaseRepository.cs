using DAL.EF;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation
{
    public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly DbSet<TEntity> _set;
        private readonly JewelryWorkshopContext _context;

        public BaseRepository(JewelryWorkshopContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<TEntity>();
        }

        public virtual async Task CreateAsync(TEntity item)
        {
            await _set.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var item = await GetAsync(id);
            if (item != null)
            {
                _set.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate)
        {
            return await Task.Run(() => _set.Where(predicate).ToList());
        }

        public virtual async Task<TEntity> GetAsync(TKey id)
        {
            return await _set.FindAsync(id);
        }    

        public virtual IQueryable<TEntity> GetAllAsQueryable()
        {
            var query = _set.AsQueryable();
            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public virtual async Task UpdateAsync(TEntity item)
        {
            _set.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
