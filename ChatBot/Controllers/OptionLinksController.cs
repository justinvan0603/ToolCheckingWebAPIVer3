using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatBot.Models;
using Microsoft.EntityFrameworkCore;
using ChatBot.Infrastructure.Core;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ChatBot.Controllers
{
    [Produces("application/json")]
    [Route("api/OptionLinks")]
    public class OptionLinksController : Controller
    {
        int _page = 1;
        int _pageSize = 10;
        private readonly DEFACEWEBSITEContext _context;
        public OptionLinksController(DEFACEWEBSITEContext context)
        {
            this._context = context;
        }
        [HttpGet]
        [Authorize(Roles = "ViewOptionLink")]
        public async Task<IEnumerable<OptionLinksNotify>> Get(string username,string domain)
        {
            //var pagination = Request.Headers["Pagination"];

            //if (!string.IsNullOrEmpty(pagination))
            //{
            //    string[] vals = pagination.ToString().Split(',');
            //    int.TryParse(vals[0], out _page);
            //    int.TryParse(vals[1], out _pageSize);
            //}
            
            //DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result = await _context.Optionlinks.FromSql("dbo.Optionlinks_Search @DOMAIN = {0} ", domain).ToListAsync();
            List<OptionLinksNotify> listOptionLinksNotify = new List<OptionLinksNotify>();
            foreach (var item in result)
            {
                OptionLinksNotify obj = new OptionLinksNotify();
                obj.Id = item.Id;
                obj.Link = item.Link;
                obj.MakerId = item.MakerId;
                obj.OptionsId = item.OptionsId;
                obj.RecordStatus = item.RecordStatus;
                obj.CreateDt = item.CreateDt;
                obj.DomainId = item.DomainId;
                string userNotificationCommand = $"dbo.UserDomainNotify_Search @USERNAME = '{username}', @DOMAIN_ID ='{item.DomainId}', @IS_TOUT = '', @IS_CIP = '', @IS_RDOM = '', @IS_ECODE = '', @IS_CCON = '' ";
                UserDomainNotify userDomainNotifyResult = await _context.UserDomainNotify.FromSql(userNotificationCommand).SingleOrDefaultAsync();
                if (userDomainNotifyResult == null)
                {
                    obj.UserDomainNotify = new UserDomainNotify();
                    obj.UserDomainNotify.DOMAIN_ID = item.DomainId;
                    obj.UserDomainNotify.USERNAME = username;
                    obj.UserDomainNotify.IS_CCON = "0";
                    obj.UserDomainNotify.IS_TOUT = "0";
                    obj.UserDomainNotify.IS_RDOM = "0";
                    obj.UserDomainNotify.IS_CIP = "0";
                    obj.UserDomainNotify.IS_ECODE = "0";
                }
                else
                {
                    obj.UserDomainNotify = userDomainNotifyResult;
                }
                listOptionLinksNotify.Add(obj);
            }
            //int currentPage = _page;
            //int currentPageSize = _pageSize;

            //var totalRecord = result.Count();
            //var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

            //var optionlinks = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
            //Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
            //IEnumerable<Optionlinks> listPagedOptionLink = Mapper.Map<IEnumerable<Optionlinks>, IEnumerable<Optionlinks>>(optionlinks);

            //context.Dispose();
            return listOptionLinksNotify;


        }
    }
}