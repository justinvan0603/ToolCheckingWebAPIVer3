using System.Collections.Generic;
using System.Linq;
using ChatBot.Data.Respositories;
using ChatBot.Model.Models;

namespace ChatBot.Service
{
    public interface IMenuRoleService
    {

        IEnumerable<MenuRole> GetAll();

        IEnumerable<MenuRole> GetAll(string keyword);

        MenuRole GetById(int id);

        void Add(MenuRole botDomain);

        void Update(MenuRole botDomain);

        void Delete(int id);

        void Save();

        MenuRole GetMenuRoleByName(string name);

    }
    public class MenuRoleService : IMenuRoleService
    {
        //  IUnitOfWork _unitOfWork;
        readonly IMenuRoleRepository _menuRoleRepository;

        public MenuRoleService(IMenuRoleRepository menuRoleRepository)
        {
            _menuRoleRepository = menuRoleRepository;
        }


        public IEnumerable<MenuRole> GetAll()
        {
            return _menuRoleRepository.GetAll();
        }

        public MenuRole GetById(int id)
        {
            return _menuRoleRepository.GetSingle(id);
        }

        public void Add(MenuRole botDomain)
        {
            _menuRoleRepository.Add(botDomain);
        }

        public void Update(MenuRole botDomain)
        {
            _menuRoleRepository.Update(botDomain);
        }

        public void Delete(int id)
        {
            _menuRoleRepository.Delete(id);

        }

        public void Save()
        {
            _menuRoleRepository.Commit();
        }

        public IEnumerable<MenuRole> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _menuRoleRepository.GetMulti(x => x.MenuName.Contains(keyword));
            else
                return _menuRoleRepository.GetAll();
        }

        public MenuRole GetMenuRoleByName(string name)
        {

            return _menuRoleRepository.GetSingleByCondition(x => x.MenuNameEl.Equals(name));
        }
    }
}