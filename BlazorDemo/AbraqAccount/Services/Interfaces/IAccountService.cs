using BlazorDemo.AbraqAccount.Models;
using BlazorDemo.Data.Issues;
using System.Threading.Tasks;

namespace BlazorDemo.AbraqAccount.Services.Interfaces;

public interface IAccountService
{
    Task<User?> AuthenticateUserAsync(string username, string password);
    Task<bool> UserExistsAsync(string username);
}

