using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.Data.Entities;
using TaskManagement.Api.Repositories.Interfaces;

namespace TaskManagement.Api.Repositories.Providers;
#nullable disable
public class AuthRepository:IAuthRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public AuthRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> CheckUserByEmailAsync(string email)
    {
        return await _dbContext.Users.AsNoTracking().AnyAsync(x => x.Email == email);
    }

    public async Task<User> GetUserByUsernameAsync(string userName)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == userName);
    }

    public async Task<bool> CheckUserByUsernameAsync(string userName)
    {
        return await _dbContext.Users.AsNoTracking().AnyAsync(x => x.Username == userName);
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        return await SaveChangesAsync();
    }
    
    private async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _dbContext.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User> GetUserAsync(string id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if (user != null)
        {
            _dbContext.Users.Remove(user);
            return await SaveChangesAsync();
        }

        return false;
    }
    
    public async Task<bool> UpdateUserAsync(User updatedUser)
    {
        _dbContext.Update(updatedUser);
        return await SaveChangesAsync();
    }
}