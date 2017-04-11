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

namespace ChatBot.Controllers
{
    [Produces("application/json")]
    [Route("api/OptionUser")]
    public class OptionUserController : Controller
    {
        int _page = 1;
        int _pageSize = 10;
        [HttpGet]
        public  IEnumerable<UserDomainSearchObject> Get(string domain)
        {
            var pagination = Request.Headers["Pagination"];

            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out _page);
                int.TryParse(vals[1], out _pageSize);
            }

            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            string command = $"dbo.Optionsuser_Search @USER = '', @DOMAIN = '{domain}'";
            var result =  context.UserDomainSearchObject.FromSql(command).ToArray();
            int currentPage = _page;
            int currentPageSize = _pageSize;

            var totalRecord = result.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

            var optionlinks = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
            Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
            IEnumerable<UserDomainSearchObject> listPagedOptionLink = Mapper.Map<IEnumerable<UserDomainSearchObject>, IEnumerable<UserDomainSearchObject>>(optionlinks);


            return listPagedOptionLink;


        }
    }
}