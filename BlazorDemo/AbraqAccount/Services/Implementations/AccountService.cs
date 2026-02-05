using Microsoft.EntityFrameworkCore;
using BlazorDemo.AbraqAccount.Data;
using BlazorDemo.AbraqAccount.Models;
using BlazorDemo.AbraqAccount.Services.Interfaces;
using System.Threading.Tasks;
using System;
using BlazorDemo.Data.Issues;

namespace BlazorDemo.AbraqAccount.Services.Implementations;

public class AccountService
{

}
//    private readonly AppDbContext _context;

//public AccountService(AppDbContext context)
//{
//    _context = context;
//}

//#region Authentication
//public async Task<User?> AuthenticateUserAsync(string username, string password)
//{
//    try
//    {
//        var user = await _context.Users
//            .FirstOrDefaultAsync(u => u.Username == username);

//        if (user == null)
//        {
//            return null;
//        }

//        Verify password(plain text comparison for now)
//            For production, use proper password hashing(BCrypt, PBKDF2, etc.)
//            if (user.Password != password)
//        {
//            return null;
//        }

//        return user;
//    }
//    catch (Exception)
//    {
//        throw;
//    }
//}

//public async Task<bool> UserExistsAsync(string username)
//{
//    try
//    {
//        return await _context.Users.AnyAsync(u => u.Username == username);
//    }
//    catch (Exception)
//    {
//        throw;
//    }
//}
//    #endregion
//}

