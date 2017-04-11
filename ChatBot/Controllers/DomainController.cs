using AutoMapper;
using ChatBot.Infrastructure.Core;
using ChatBot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatBot.Controllers
{
    [Route("api/[controller]")]
    public class DomainsController : Controller
    {
        private int _page = 1;
        private int _pageSize = 10;
        /*
        @USER varchar(15) = NULL,
	    @DOMAIN varchar(500) = NULL,
	    @RECORD_STATUS varchar(1) = NULL,
	    @CREATE_DT varchar(20) = NULL
         */

        [HttpGet]
        public async Task<IEnumerable<ListdomainObject>> Get(string userid)
        {
            var pagination = Request.Headers["Pagination"];

            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out _page);
                int.TryParse(vals[1], out _pageSize);
            }

            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            string command = $"dbo.Listdomain_Search @USER = '{userid}', @DOMAIN = '',@RECORD_STATUS = '1',@CREATE_DT=''";
            var result = await context.ListdomainObject.FromSql(command).ToArrayAsync();
            int currentPage = _page;
            int currentPageSize = _pageSize;

            var totalRecord = result.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

            var domains = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
            Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
            IEnumerable<ListdomainObject> listPagedDomain = Mapper.Map<IEnumerable<ListdomainObject>, IEnumerable<ListdomainObject>>(domains);

            return listPagedDomain;
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            string command = $"dbo.Listdomain_Del @ID={id}";
            var result = await context.Database.ExecuteSqlCommandAsync(command, cancellationToken: CancellationToken.None);
            return result;
        }

        /*
         @p_DOMAIN	varchar(200)  = NULL,
@p_USER_ID	varchar(15)  = NULL,
@p_USERNAME	varchar(50)  = NULL,
@p_DESCRIPTION	nvarchar(500)  = NULL,
@p_RECORD_STATUS	varchar(1)  = NULL,
@p_AUTH_STATUS	varchar(1)  = NULL,
@p_CREATE_DT	VARCHAR(20) = NULL,
@p_APPROVE_DT	VARCHAR(20) = NULL,
@p_EDIT_DT	VARCHAR(20) = NULL,
@p_MAKER_ID	varchar(15)  = NULL,
@p_CHECKER_ID	varchar(15)  = NULL,
@p_EDITOR_ID	varchar(15)  = NULL
         */

        [HttpPost]
        public async Task<int> Post([FromBody]ListdomainObject domain)
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            string command = $"dbo.Listdomain_Ins @p_DOMAIN = '{domain.DOMAIN}',@p_USER_ID = '{domain.USER_ID}',@p_USERNAME='{domain.USERNAME}',@p_DESCRIPTION=N'{domain.DESCRIPTION}',@p_RECORD_STATUS='1',@p_AUTH_STATUS ='U',@p_CREATE_DT = '{DateTime.Now.Date}',@p_APPROVE_DT = '',@p_EDIT_DT ='',@p_MAKER_ID = 'thieu1234',@p_CHECKER_ID ='',@p_EDITOR_ID=''";
            var result = await context.Database.ExecuteSqlCommandAsync(command, cancellationToken: CancellationToken.None);
            return result;
        }

        /*
         @p_ID	int = NULL,
@p_DOMAIN	varchar(200) = NULL ,
@p_USER_ID	varchar(15) = NULL ,
@p_USERNAME	varchar(50) = NULL ,
@p_DESCRIPTION	nvarchar(500) = NULL ,
@p_RECORD_STATUS	varchar(1) = NULL ,
@p_AUTH_STATUS	varchar(1) = NULL ,
@p_CREATE_DT	VARCHAR(20) = NULL,
@p_APPROVE_DT	VARCHAR(20) = NULL,
@p_EDIT_DT	VARCHAR(20) = NULL,
@p_MAKER_ID	varchar(15) = NULL ,
@p_CHECKER_ID	varchar(15) = NULL ,
@p_EDITOR_ID	varchar(15) = NULL
         */

        [HttpPut("{id}")]
        public async Task<int> Put(int id, [FromBody]ListdomainObject domain)
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            string command = $"dbo.Listdomain_Upd @p_ID= {domain.ID},@p_DOMAIN = '{domain.DOMAIN}',@p_USER_ID='{domain.USER_ID}',@p_USERNAME='{domain.USERNAME}',@p_DESCRIPTION = N'{domain.DESCRIPTION}',@p_RECORD_STATUS = '{domain.RECORD_STATUS}',@p_AUTH_STATUS = '{domain.AUTH_STATUS}',@p_CREATE_DT = '{domain.CREATE_DT}',@p_APPROVE_DT = '{domain.APPROVE_DT}',@p_EDIT_DT = '{DateTime.Now.Date}',@p_MAKER_ID = '{domain.MAKER_ID}',@p_CHECKER_ID = '{domain.CHECKER_ID}',@p_EDITOR_ID = '{domain.EDITOR_ID}'";
            var result = await context.Database.ExecuteSqlCommandAsync(command, cancellationToken: CancellationToken.None);
            return result;
        }
    }
}