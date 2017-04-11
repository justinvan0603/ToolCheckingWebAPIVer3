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
    [Route("api/OptionLinks")]
    public class OptionLinksController : Controller
    {
        int _page = 1;
        int _pageSize = 10;
        [HttpGet]
        public async Task<IEnumerable<Optionlinks>> Get(string domain)
        {
            var pagination = Request.Headers["Pagination"];

            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out _page);
                int.TryParse(vals[1], out _pageSize);
            }

            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result = await context.Optionlinks.FromSql("dbo.Optionlinks_Search @DOMAIN = {0} ", domain).ToArrayAsync();
            int currentPage = _page;
            int currentPageSize = _pageSize;

            var totalRecord = result.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

            var optionlinks = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
            Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
            IEnumerable<Optionlinks> listPagedOptionLink = Mapper.Map<IEnumerable<Optionlinks>, IEnumerable<Optionlinks>>(optionlinks);


            return listPagedOptionLink;


        }
    }
}