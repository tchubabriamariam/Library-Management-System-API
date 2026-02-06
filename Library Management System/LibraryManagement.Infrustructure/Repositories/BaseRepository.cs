using System.Linq.Expressions;
using LibraryManagement.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrustructure.Repositories
{
    public class BaseRepository<T> where T : class
    {
        #region Protected

        protected readonly LibraryManagementContext _context;
        protected readonly DbSet<T> _dbSet;

        #endregion

        #region Ctor

        public BaseRepository(LibraryManagementContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        #endregion

        #region Methods

        public async Task<List<T>> GetAllAsync(CancellationToken token)
        {
            return await _dbSet.ToListAsync(token);
        }

        public async Task<T?> GetAsync(CancellationToken token, params object[] key)
        {
            return await _dbSet.FindAsync(key, token);
        }

        public async Task AddAsync(CancellationToken token, T entity)
        {
            await _dbSet.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(CancellationToken token, T entity)
        {
            if (entity == null)
                return;

            _dbSet.Update(entity);
            await _context.SaveChangesAsync(token);
        }

        public async Task RemoveAsync(CancellationToken token, T entity)
        {
            if (entity == null)
                return;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(token);
        }

        public async Task RemoveAsync(CancellationToken token, params object[] key)
        {
            var entity = await GetAsync(token, key);

            if (entity == null)
                return;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(token);
        }

        public Task<bool> AnyAsync(CancellationToken token, Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AnyAsync(predicate, token);
        }

        #endregion
        
        
        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }
    }
}
