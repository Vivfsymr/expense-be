using ExpenseBe.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseBe.API.DTOs;
namespace ExpenseBe.Core.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(string id, User user);
        Task<bool> DeleteUserAsync(string id);
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
} 