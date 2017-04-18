using ChatBot.Data.Infrastructure;
using ChatBot.Model.Models;

namespace ChatBot.Data.Respositories
{
    public interface IBotDomainRepository : IRepositoryBase<BOT_DOMAIN>
    {
    }

    public class BotDomainRepository : RepositoryBase<BOT_DOMAIN>, IBotDomainRepository
    {
        public BotDomainRepository(ChatBotDbContext context)
            : base(context)
        { }
    }
}
