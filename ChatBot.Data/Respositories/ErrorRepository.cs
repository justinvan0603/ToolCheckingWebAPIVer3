using ChatBot.Data.Infrastructure;
using ChatBot.Model.Models;

namespace ChatBot.Data.Respositories
{
    public class LoggingRepository : RepositoryBase<Error>, ILoggingRepository
    {
        public LoggingRepository(ChatBotDbContext context)
            : base(context)
        { }

        public override void Commit()
        {
            try
            {
                base.Commit();
            }
            catch { }
        }
    }
}
