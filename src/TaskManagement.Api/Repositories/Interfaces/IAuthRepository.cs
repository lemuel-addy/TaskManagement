using TaskManagement.Api.Data.Entities;
#nullable disable
namespace TaskManagement.Api.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<User> GetUserByEmailAsync(string email);
    Task<bool> CheckUserByEmailAsync(string email);
    Task<User> GetUserByUsernameAsync(string userName);
    Task<bool> CheckUserByUsernameAsync(string userName);
    Task<bool> CreateUserAsync(User user);
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserAsync(string id);
    Task<bool> DeleteUserAsync(string id);
    Task<bool> UpdateUserAsync(User updatedUser);
}