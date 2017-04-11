using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DefaceWebsiteService;
using Microsoft.AspNetCore.Cors;
using ChatBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Data.SqlClient;
using ChatBot.Infrastructure.Core;
using ChatBot.Infrastructure.MD5Encryption;
using System.Xml.Linq;
using AutoMapper;

namespace ChatBot.Controllers
{

    // [EnableCors("CorsPolicy")]
    // [Produces("application/json")]
    //[Route("api/Users")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        int _page = 1;
        int _pageSize = 10;
        [HttpGet]
        public async Task<IEnumerable<Users>> Get(int parentid, string parentname)
        {
            var pagination = Request.Headers["Pagination"];

            if (!string.IsNullOrEmpty(pagination))
            {
                string[] vals = pagination.ToString().Split(',');
                int.TryParse(vals[0], out _page);
                int.TryParse(vals[1], out _pageSize);
            }

            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result = await context.Users.FromSql("dbo.Users_ByParent @p_PARENT_ID = {0}, @p_PARENT_NAME = {1}", 1056, "thieu1234").ToArrayAsync();
            int currentPage = _page;
            int currentPageSize = _pageSize;

            var totalRecord = result.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

            var users = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
            Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
            IEnumerable<Users> listPagedUser = Mapper.Map<IEnumerable<Users>, IEnumerable<Users>>(users);
            

            return listPagedUser;


        }
        [HttpGet]
        [Route("GetListByParent")]
        public async Task<IEnumerable<Users>> GetListByParent(int parentid, string parentname)
        {
            var pagination = Request.Headers["Pagination"];


            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result = await context.Users.FromSql("dbo.Users_ByParent @p_PARENT_ID = {0}, @p_PARENT_NAME = {1}", 1056, "thieu1234").ToArrayAsync();
            //int currentPage = _page;
            //int currentPageSize = _pageSize;

            //var totalRecord = result.Count();
            //var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

            //var users = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
            //Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
            //IEnumerable<Users> listPagedUser = Mapper.Map<IEnumerable<Users>, IEnumerable<Users>>(users);


            return result;


        }
        [HttpGet]
        [Route("GetUserById")]
        public async Task<Users> GetUserById(int? userid, string username)
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result = await context.Users.FromSql("dbo.Users_ById @p_USERNAME = {0}, @p_USERID = {1}", username, userid).SingleAsync();
            return result;
        }
        [HttpPut]
        [Route("ChangePassword")]
        public  int ChangePassword(string username, string currentpassword, string newpassword)
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result =  context.Users.FromSql("dbo.Users_ById @p_USERNAME = {0}, @p_USERID = null ", username).Single();
            if(MD5Encoder.MD5Hash(currentpassword).Equals(result.Password))
            {
                result.Password = MD5Encoder.MD5Hash(newpassword);
                string command = $"dbo.Users_Upd @p_ID={result.Id},@p_USERNAME = '{result.Username}',@p_FULLNAME=N'{result.Fullname}',@p_PASSWORD='{result.Password}',@p_EMAIL = '{result.Email}',@p_PHONE={result.Phone},@p_PARENT_ID='{result.ParentId}',@p_DESCRIPTION=N'{result.Description}',@p_RECORD_STATUS='{result.RecordStatus}',@p_AUTH_STATUS='{result.AuthStatus}',@p_CREATE_DT='{result.CreateDt}',@p_APPROVE_DT='{result.ApproveDt}',@p_EDIT_DT='{DateTime.Now.Date}',@p_MAKER_ID='{result.MakerId}',@p_CHECKER_ID='{result.CheckerId}',@p_EDITOR_ID='{result.EditorId}'";
                int updResult =  context.Database.ExecuteSqlCommand(command);
                return updResult;
            }
            else
            {
                return -1;
            }
        }
        [HttpPost]
        public  int Post([FromBody]UserObject user)
        {
            user.ParentId = null;
            user.RecordStatus = "1";
            user.AuthStatus = "U";
            user.ApproveDt = null;
            user.EditDt = null;
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            string pass = MD5Encoder.MD5Hash(user.Password);
            XElement xmldata = new XElement(new XElement("Root"));
            XElement x = new XElement("Domain", new XElement("DOMAIN", user.Domain),
                                               new XElement("DESCRIPTION", user.DomainDesc));
            xmldata.Add(x);

            string command = $"dbo.Users_Ins @p_USERNAME = '{user.Username}', @p_FULLNAME= N'{user.Fullname}',@p_PASSWORD = '{pass}',@p_EMAIL = '{user.Email}',@p_PHONE = {user.Phone},@p_PARENT_ID = '',@p_DESCRIPTION = N'{user.Description}',@p_RECORD_STATUS = '{user.RecordStatus}',@p_AUTH_STATUS = '{user.AuthStatus}',@p_CREATE_DT = '{DateTime.Now.Date}',@p_APPROVE_DT = '{user.ApproveDt}' ,@p_EDIT_DT= '{user.EditDt}' ,@p_MAKER_ID ='{user.MakerId}',@p_CHECKER_ID = '{user.CheckerId}',@p_EDITOR_ID = '{user.EditorId}',@DOMAIN ='{xmldata}'";
            var result =  context.Database.ExecuteSqlCommand(command);
            return result;
        }
        /*
         * @p_USERNAME	varchar(50)  = NULL,
            @p_FULLNAME	nvarchar(200)  = NULL,
            @p_PASSWORD	varchar(50)  = NULL,
            @p_EMAIL	varchar(50)  = NULL,
            @p_PHONE	int = NULL,
            @p_PARENT_ID	int = NULL,
            @p_DESCRIPTION	nvarchar(500)  = NULL,
            @p_RECORD_STATUS	varchar(1)  = NULL,
            @p_AUTH_STATUS	varchar(1)  = NULL,
            @p_CREATE_DT	VARCHAR(20) = NULL,
            @p_APPROVE_DT	VARCHAR(20) = NULL,
            @p_EDIT_DT	VARCHAR(20) = NULL,
            @p_MAKER_ID	varchar(15)  = NULL,
            @p_CHECKER_ID	varchar(15)  = NULL,
            @p_EDITOR_ID	varchar(15)  = NULL,
            @DOMAIN XML = NULL
         */


        /*@p_ID	int = NULL,
@p_USERNAME	varchar(50)  = NULL,
@p_FULLNAME	nvarchar(200)  = NULL,
@p_PASSWORD	varchar(50)  = NULL,
@p_EMAIL	varchar(50)  = NULL,
@p_PHONE	int = NULL,
@p_PARENT_ID	int = NULL,
@p_DESCRIPTION	nvarchar(500)  = NULL,
@p_RECORD_STATUS	varchar(1)  = NULL,
@p_AUTH_STATUS	varchar(1)  = NULL,
@p_CREATE_DT	VARCHAR(20) = NULL,
@p_APPROVE_DT	VARCHAR(20) = NULL,
@p_EDIT_DT	VARCHAR(20) = NULL,
@p_MAKER_ID	varchar(15)  = NULL,
@p_CHECKER_ID	varchar(15)  = NULL,
@p_EDITOR_ID	varchar(15)  = NULL,
@DOMAIN XML = NULL,*/
        [HttpPut("{id}")]
        public async Task<int> Put(int id,[FromBody]Users user)
        {
            user.AuthStatus = "U";
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            string password = MD5Encoder.MD5Hash(user.Password);
            string command = $"dbo.Users_Upd @p_ID={user.Id},@p_USERNAME = '{user.Username}',@p_FULLNAME=N'{user.Fullname}',@p_PASSWORD='{password}',@p_EMAIL = '{user.Email}',@p_PHONE={user.Phone},@p_PARENT_ID={user.ParentId},@p_DESCRIPTION=N'{user.Description}',@p_RECORD_STATUS='{user.RecordStatus}',@p_AUTH_STATUS='{user.AuthStatus}',@p_CREATE_DT='{user.CreateDt}',@p_APPROVE_DT='{user.ApproveDt}',@p_EDIT_DT='{DateTime.Now.Date}',@p_MAKER_ID='{user.MakerId}',@p_CHECKER_ID='{user.CheckerId}',@p_EDITOR_ID='{user.EditorId}'";
            var result = await context.Database.ExecuteSqlCommandAsync(command, cancellationToken: CancellationToken.None);
            return result;
        }

        //[Route("{username?}/{userid}", Name = "GetUserById")]
        //public async Task<IEnumerable<Users>> Get(string username, int userid)
        //{
            
        //    DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
        //    var result = await context.Users.FromSql("dbo.Users_ById @p_USERNAME = {0}, @p_USERID = {1}", username, userid).ToArrayAsync();
        //    return result;
        //}
        //[HttpGet("{id:int}", Name = "GetUserByParent")]
        //public async Task<IEnumerable<Users>> GetByParentId(int id)
        //{
        //    DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
        //    var result = await context.Users.FromSql("dbo.Users_ByParent @p_USERNAME = {0}, @p_USERID = {1}", "thieu1234", 1056).ToArrayAsync();
        //    return result;
        //}
        //[Route("Delete")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result = await context.Database.ExecuteSqlCommandAsync("Users_Del @ID = {0}", cancellationToken: CancellationToken.None,parameters: id);
            //return result;
            GenericResult removeResult = new GenericResult();
            if (result == 1)
            {
                removeResult = new GenericResult()
                {
                    Succeeded = true,
                    Message = "Users removed."
                };
            }
            else
            {
                removeResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = "Failed to delete"
                };
            }
            ObjectResult deleteResult = new ObjectResult(removeResult);
            return deleteResult;
        }
    }
}