using System.Collections.Generic;
using ChatBot.Model.Models;

namespace ChatBot.Data.Infrastructure
{

    public interface ILoggingRepository : IRepositoryBase<Error> { }
    //public interface IRoleRepository : IRepositoryBase<Role> { }

    //public interface IUserRepository : IRepositoryBase<User>
    //{
    //    User GetSingleByUsername(string username);
    //    IEnumerable<Role> GetUserRoles(string username);
    //}

    //public interface IUserRoleRepository : IRepositoryBase<UserRole> { }
}
