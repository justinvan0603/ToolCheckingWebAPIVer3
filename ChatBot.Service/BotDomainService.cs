//using System.Collections.Generic;
//using System.Linq;
//using ChatBot.Data.Respositories;
//using ChatBot.Model.Models;

//namespace ChatBot.Service
//{
//    public interface IBotDomainService
//    {

//        IEnumerable<BOT_DOMAIN> GetAll();

//        IEnumerable<BOT_DOMAIN> GetAll(string keyword);
//        BOT_DOMAIN GetById(int id);

//        void Add(BOT_DOMAIN botDomain);

//        void Update(BOT_DOMAIN botDomain);

//        void Delete(int id);

//        void Save();

//    }
//    public class BotDomainService : IBotDomainService
//    {
//        //  IUnitOfWork _unitOfWork;
//        readonly IBotDomainRepository _botDomainRepository;

//        public BotDomainService(IBotDomainRepository botDomainRepository)
//        {
//            _botDomainRepository = botDomainRepository;
//        }


//        public IEnumerable<BOT_DOMAIN> GetAll()
//        {
//            return _botDomainRepository.GetAll().Where(x => x.Status);
//        }

//        public BOT_DOMAIN GetById(int id)
//        {
//            return _botDomainRepository.GetSingle(id);
//        }

//        public void Add(BOT_DOMAIN botDomain)
//        {
//            _botDomainRepository.Add(botDomain);
//        }

//        public void Update(BOT_DOMAIN botDomain)
//        {
//            _botDomainRepository.Edit(botDomain);
//        }

//        public void Delete(int id)
//        {
//            _botDomainRepository.Delete(id);

//        }

//        public void Save()
//        {
//            _botDomainRepository.Commit();
//        }

//        public IEnumerable<BOT_DOMAIN> GetAll(string keyword)
//        {
//            if (!string.IsNullOrEmpty(keyword))
//                return _botDomainRepository.GetMulti(x => x.DOMAIN.Contains(keyword));
//            else
//                return _botDomainRepository.GetAll();
//        }
//    }
//}