using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AutoService_Master.Models;

namespace AutoService_Master.Services;

public class AuthService
{
    public static AuthService Instance { get; } = new AuthService();
    public User? CurrentUser { get; private set; }

    public bool IsAdmin => CurrentUser?.Role == UserRole.Admin;

    private readonly List<User> _users = new()
    {
        new User { Username = "admin", Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", Role = UserRole.Admin },
        new User { Username = "user", Password = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3", Role = UserRole.User }
    };

    public bool Login(string username, string password)
    {
        string hashedPassword = HashPassword(password);

        var user = _users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);
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

    private string HashPassword(string rawPassword)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawPassword));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}