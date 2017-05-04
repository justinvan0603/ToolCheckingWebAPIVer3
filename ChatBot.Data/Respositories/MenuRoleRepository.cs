using ChatBot.Data.Infrastructure;
using ChatBot.Model.Models;

namespace ChatBot.Data.Respositories
{
    public interface IMenuRoleRepository : IRepositoryBase<MenuRole>
    {
    }

    public class MenuRoleRepository : RepositoryBase<MenuRole>, IMenuRoleRepository
    {
        public MenuRoleRepository(ChatBotDbContext context)
            : base(context)
        { }
    }
}
