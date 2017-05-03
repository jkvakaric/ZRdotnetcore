using System;
using System.Collections.Generic;
using ZRdotnetcore.Models;

namespace ZRdotnetcore.Repos.Interfaces
{
    public interface IUserRepo
    {
        User Get(Guid userId);
        void Add(User user);
        void Update(User user);
        bool CheckUsernameExists(string username);
        List<User> GetUserList(List<Guid> guids);
    }
}