using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ZRdotnetcore.Data;
using ZRdotnetcore.Models;
using ZRdotnetcore.Repos.Interfaces;

namespace ZRdotnetcore.Repos
{
    internal class UserRepo : IUserRepo
    {
        private readonly YoctoDbContext _context;

        public UserRepo(YoctoDbContext context)
        {
            _context = context;
        }

        public User Get(string userId)
        {
            User user = _context.Users.Find(userId);
            return user;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool CheckUsernameExists(string username)
        {
            return _context.Users.Any(u => u.Username.Equals(username));
        }

        public List<User> GetUserList(List<string> ids)
        {
            var list = _context.Users.Where(u => ids.Contains(u.UserId))
                .OrderBy(u => u.Email).ToList();
            return list;
        }
    }
    
}