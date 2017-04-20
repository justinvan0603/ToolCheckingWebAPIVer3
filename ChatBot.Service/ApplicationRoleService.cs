using System.Collections.Generic;
using System.Linq;
using ChatBot.Data.Respositories;
using ChatBot.Model.Models;
using ChatBot.Service.Exceptions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChatBot.Service
{
    public interface IApplicationRoleService
    {
        IdentityRole GetDetail(string id);

        IEnumerable<IdentityRole> GetAll(int page, int pageSize, out int totalRow, string filter);

        IEnumerable<IdentityRole> GetAll();

        EntityEntry<IdentityRole> Add(IdentityRole appRole);

        void Update(IdentityRole AppRole);

        void Delete(string id);

        //Add roles to a sepcify group
        bool AddRolesToGroup(IEnumerable<ApplicationRoleGroup> roleGroups, int groupId);

        //Get list role by group id
        IEnumerable<IdentityRole> GetListRoleByGroupId(int groupId);

        void Save();
    }

    public class ApplicationRoleService : IApplicationRoleService
    {
        private IApplicationRoleRepository _appRoleRepository;
        private IApplicationRoleGroupRepository _appRoleGroupRepository;
   //     private IUnitOfWork _unitOfWork;

        public ApplicationRoleService(
            IApplicationRoleRepository appRoleRepository, IApplicationRoleGroupRepository appRoleGroupRepository)
        {
            this._appRoleRepository = appRoleRepository;
            this._appRoleGroupRepository = appRoleGroupRepository;
         //   this._unitOfWork = unitOfWork;
        }

        public EntityEntry<IdentityRole> Add(IdentityRole appRole)
        {
            if (_appRoleRepository.CheckContains(x => x.Name == appRole.Name))
                throw new NameDuplicatedException("Tên không được trùng");
            return _appRoleRepository.Add(appRole);
        }

        public bool AddRolesToGroup(IEnumerable<ApplicationRoleGroup> roleGroups, int groupId)
        {

            _appRoleGroupRepository.DeleteMulti(x => x.GroupId == groupId);
            _appRoleGroupRepository.Commit();
            foreach (var roleGroup in roleGroups)
            {
                _appRoleGroupRepository.Add(roleGroup);
            }
            return true;
        }

        public void Delete(string id)
        {
            _appRoleRepository.DeleteMulti(x => x.Id == id);
        }

        public IEnumerable<IdentityRole> GetAll()
        {
            return _appRoleRepository.GetAll();
        }

        public IEnumerable<IdentityRole> GetAll(int page, int pageSize, out int totalRow, string filter = null)
        {
            var query = _appRoleRepository.GetAll();
            //if (!string.IsNullOrEmpty(filter))
            //    query = query.Where(x => x.Description.Contains(filter));

            totalRow = query.Count();
            return query.Skip(page * pageSize).Take(pageSize);
        }

        public IdentityRole GetDetail(string id)
        {
            return _appRoleRepository.GetSingleByCondition(x => x.Id == id);
        }

        public void Save()
        {
            _appRoleRepository.Commit();
        }

        public void Update(IdentityRole AppRole)
        {
            if (_appRoleRepository.CheckContains(x => x.Name == AppRole.Name && x.Id != AppRole.Id))
                throw new NameDuplicatedException("Tên không được trùng");
            _appRoleRepository.Update(AppRole);
        }

        public IEnumerable<IdentityRole> GetListRoleByGroupId(int groupId)
        {
            return _appRoleRepository.GetListRoleByGroupId(groupId);
        }
    }
}
