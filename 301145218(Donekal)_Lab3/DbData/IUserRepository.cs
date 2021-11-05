
using _301145218_Donekal__Lab3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _301145218_Donekal__Lab3.DbData
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        void SaveUser(User user);
        User GetUserMovies(string email);

    }
}
