
using Microsoft.EntityFrameworkCore;
using XUnitTestProjectWebApp.Context;

namespace XUnitTestProjectWebApp.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ProductContext _context;
        private readonly DbSet<TEntity> _dbbSet;

        public Repository(ProductContext context)
        {
            _context = context;
            _dbbSet = _context.Set<TEntity>();
        }

        public async Task Create(TEntity entity)
        {
            await _dbbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            _dbbSet.Remove(entity);
            _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbbSet.ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await _dbbSet.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
           // _dbbSet.Update(entity);
            _context.SaveChanges();
        }

    }
}