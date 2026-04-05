using FacultySports.Domain.Entities;
using FacultySports.Infrastructure.Context;
using FacultySports.Infrastructure.Repositories.Interfaces;
using FacultySports.Infrastructure.Repositories.Realizations.Base;
using Microsoft.EntityFrameworkCore;

namespace FacultySports.Infrastructure.Repositories.Realizations;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(SportsDbContext context) : base(context)
    {
    }

    public override async Task<User?> GetByIdAsync(int id)
    {
        throw new NotSupportedException("Use GetByIdAsync(string id)");
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task CreateAsync(User user)
    {
        await _dbSet.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        _dbSet.Update(user);
    }

    public async Task DeleteAsync(string id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            _dbSet.Remove(user);
        }
    }
}