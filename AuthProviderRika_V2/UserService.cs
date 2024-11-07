using AuthProviderRika_V2.Contexts;
using AuthProviderRika_V2.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthProviderRika_V2;

public class UserService(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task<UserEntity> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user != null)
        {
            return user;
        }
        return null!;

    }

    public async Task<UserEntity> GetUserById(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user != null)
        {
            return user;
        }
        return null!;
    }


    public async Task CreateUser(UserEntity user)
    {
        _context.Users.Add((Entities.UserEntity)user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(UserEntity user)
    {
        var existingUser = await GetUserByEmail(user.Id);
        if (existingUser != null)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new KeyNotFoundException("User not found ");
        }

    }

    public async Task DeleteUser(string Username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == Username);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

}
