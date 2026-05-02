using System.Collections.Generic;
using System.Linq;
using AutoService_Master.Models;

namespace AutoService_Master.Services;

public class AuthService
{
    public static AuthService Instance { get; } = new AuthService();
    public User? CurrentUser { get; private set; }
    
    public bool IsAdmin => CurrentUser?.Role == UserRole.Admin;
    
    private readonly List<User> _users = new()
    {
        new User { Username = "admin", Password = "123", Role = UserRole.Admin },
        new User { Username = "user", Password = "123", Role = UserRole.User }
    };

    public bool Login(string username, string password)
    {
        var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user != null)
        {
            CurrentUser = user;
            return true; 
        }
        return false; 
    }

    public void Logout()
    {
        CurrentUser = null;
    }
}