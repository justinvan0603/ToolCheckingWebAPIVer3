using System.Collections.Generic;
using ChatBot.Data.Infrastructure;
using ChatBot.Model.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChatBot.Data.Respositories
{
    public interface IApplicationRoleRepository : IRepositoryBase<IdentityRole>
    {
        IEnumerable<IdentityRole> GetListRoleByGroupId(int groupId);
    }
    public class ApplicationRoleRepository : RepositoryBase<IdentityRole>, IApplicationRoleRepository
    {
     

        public ApplicationRoleRepository(ChatBotDbContext context)
            : base(context)
        { }

        public IEnumerable<IdentityRole> GetListRoleByGroupId(int groupId)
        {
            var query = from g in DbContext.Roles
                        join ug in DbContext.ApplicationRoleGroups
                        on g.Id equals ug.RoleId
                        where ug.GroupId == groupId
                        select g;
            return query;
        }
    }
}
