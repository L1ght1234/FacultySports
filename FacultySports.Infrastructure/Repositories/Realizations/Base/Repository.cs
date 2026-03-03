using Microsoft.EntityFrameworkCore;
using FacultySports.Infrastructure.Context;
using FacultySports.Infrastructure.Repositories.Interfaces.Base;

namespace FacultySports.Infrastructure.Repositories.Realizations.Base;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly SportsDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(SportsDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return entity;
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
