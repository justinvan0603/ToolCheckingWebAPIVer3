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
    public class DomainsController : Controller//, IDisposable
    {
        private int _page = 1;
        private int _pageSize = 10;
        private readonly DEFACEWEBSITEContext _context;
        //protected override void Dispose(bool disposing)
        //{
        //    _context.Dispose();
            
        //}
        public DomainsController(DEFACEWEBSITEContext context)
        {
            _context = context;
        }
        [HttpGet]
        public  async Task<IEnumerable<ListdomainObject>> Get(string userid, string searchString = "")
        {

            //using (DEFACEWEBSITEContext context = new DEFACEWEBSITEContext())
            //{
                try
                {
                    var pagination = Request.Headers["Pagination"];

                    if (!string.IsNullOrEmpty(pagination))
                    {
                        string[] vals = pagination.ToString().Split(',');
                        int.TryParse(vals[0], out _page);
                        int.TryParse(vals[1], out _pageSize);
                    }


                    string command = $"dbo.Listdomain_SearchByUsername @USERNAME = '{userid}', @DOMAIN = '',@RECORD_STATUS = '1',@CREATE_DT=''";
                    var result =   _context.ListdomainObject.FromSql(command);

                    if (!String.IsNullOrEmpty(searchString))
                    {
                    result = result.Where(domain => domain.DOMAIN.ToLower().Contains(searchString.ToLower()) ||
                    domain.USERNAME.ToLower().Contains(searchString.ToLower()) ||
                    domain.DESCRIPTION.ToLower().Contains(searchString.ToLower()) ||
                    domain.FULLNAME.ToLower().Contains(searchString.ToLower()));
                    }
                    var totalRecord = result.Count();
                    var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

                   
                    
                    Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
                //Added these line and the issue gone!
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                //IEnumerable<ListdomainObject> listPagedDomain = Mapper.Map<IEnumerable<ListdomainObject>, IEnumerable<ListdomainObject>>(domains);
                //_context.Dispose();
                //_context.Dispose();
                    return await result.Skip((_page - 1) * _pageSize).Take(_pageSize).ToListAsync();
                }
                catch (Exception ex)
                {

                    return new List<ListdomainObject>();
                }
            //}
    
            
        }

        [HttpDelete("{id}")]
        public async Task<ObjectResult> Delete(int id)
        {
            GenericResult rs = new GenericResult();
            //DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            try
            {
                
                string command = $"dbo.Listdomain_Del @ID={id}";
                var result = await _context.Database.ExecuteSqlCommandAsync(command, cancellationToken: CancellationToken.None);
                rs.Succeeded = true;
                rs.Message = "Xóa domain thành công!";
                //return result;
                ObjectResult objRes = new ObjectResult(rs);
                //context.Dispose();
                return objRes;
            }
            catch(Exception ex)
            {
                rs.Succeeded = false;
                rs.Message = "Lỗi: " + ex.Message;
                ObjectResult objRes = new ObjectResult(rs);
                //context.Dispose();
                return objRes;
            }

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
        public async Task<ObjectResult> Post([FromBody]ListdomainObject domain)
        {
            GenericResult rs = new GenericResult();
            //DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            try
            {
                
                string command = $"dbo.Listdomain_Ins @p_DOMAIN = '{domain.DOMAIN}',@p_USER_ID = '{domain.USER_ID}',@p_USERNAME='{domain.USERNAME}',@p_DESCRIPTION=N'{domain.DESCRIPTION}',@p_RECORD_STATUS='1',@p_AUTH_STATUS ='U',@p_CREATE_DT = '{DateTime.Now.Date}',@p_APPROVE_DT = '',@p_EDIT_DT ='',@p_MAKER_ID = 'thieu1234',@p_CHECKER_ID ='',@p_EDITOR_ID=''";
                var result = await _context.Database.ExecuteSqlCommandAsync(command, cancellationToken: CancellationToken.None);
                //return result;
                rs.Message = "Thêm domain thành công";
                rs.Succeeded = true;
                ObjectResult objRes = new ObjectResult(rs);
                //context.Dispose();
                return objRes;
                
            }
            catch(Exception ex)
            {
                rs.Message = "Lỗi " + ex.Message;
                rs.Succeeded = false;
                ObjectResult objRes = new ObjectResult(rs);
                //context.Dispose();
                return objRes;
            }
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
        public async Task<ObjectResult> Put(int id, [FromBody]ListdomainObject domain)
        {
            GenericResult rs = new GenericResult();
            //DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            try
            {
                
                string command = $"dbo.Listdomain_Upd @p_ID= {domain.ID},@p_DOMAIN = '{domain.DOMAIN}',@p_USER_ID='{domain.USER_ID}',@p_USERNAME='{domain.USERNAME}',@p_DESCRIPTION = N'{domain.DESCRIPTION}',@p_RECORD_STATUS = '{domain.RECORD_STATUS}',@p_AUTH_STATUS = '{domain.AUTH_STATUS}',@p_CREATE_DT = '{domain.CREATE_DT}',@p_APPROVE_DT = '{domain.APPROVE_DT}',@p_EDIT_DT = '{DateTime.Now.Date}',@p_MAKER_ID = '{domain.MAKER_ID}',@p_CHECKER_ID = '{domain.CHECKER_ID}',@p_EDITOR_ID = '{domain.EDITOR_ID}'";
                var result = await _context.Database.ExecuteSqlCommandAsync(command, cancellationToken: CancellationToken.None);
                rs.Succeeded = true;
                rs.Message = "Cập nhật Domain thành công";
                ObjectResult objRes = new ObjectResult(rs);
                //context.Dispose();
                return objRes;
            }
            catch(Exception ex)
            {
                rs.Succeeded = false;
                rs.Message = ex.Message;
                ObjectResult objRes = new ObjectResult(rs);
                //context.Dispose();
                return objRes;
            }
        }
    }
}