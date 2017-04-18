using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using System.Threading;
using ChatBot.Infrastructure.Core;

namespace ChatBot.Controllers
{
    [Produces("application/json")]
    [Route("api/UserConfigs")]
    public class UserConfigsController : Controller
    {
        private readonly DEFACEWEBSITEContext _context;
        public UserConfigsController(DEFACEWEBSITEContext context)
        {
            this._context = context;
        }
        [HttpGet("{id}", Name = "GetConfig")]
        public async Task<IEnumerable<UserConfigObject>> Get(string id)
        {
            //using (DEFACEWEBSITEContext context = new DEFACEWEBSITEContext())
            //{
           // try
           // {
                var result = await _context.UserConfigObject.FromSql("dbo.Configtype_ByUser @USER = {0}", id).ToArrayAsync();
                //context.Dispose();
                return result;
           // }
           // catch(Exception ex)
           // {
            //    int a = 5;
           //     return null;
          //  }
            //}
        }
        [HttpPut("{id}")]
        public async Task<ObjectResult> Put(string id, [FromBody]List<UserConfigObject> user)
        {
            GenericResult rs = new GenericResult();
            //DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            try
            {
                XElement xmldata = new XElement(new XElement("Root"));

                foreach (var item in user)
                {
                    if (item.VALUE_TYPE.Equals("CHECKBOX"))
                    {
                        if (item.CONF_VALUE.Equals("true"))
                        {
                            item.CONF_VALUE = "1";
                        }
                        else if (item.CONF_VALUE.Equals("false"))
                        {
                            item.CONF_VALUE = "0";
                        }
                    }
                    XElement x = new XElement("Config", new XElement("CONF_TYPE", item.CONF_TYPE),
                                                   new XElement("CONF_VALUE", item.CONF_VALUE));
                    xmldata.Add(x);

                }
                
                string command = $"dbo.Userconfig_Upd @USER = '{id}', @CONFIG = '{xmldata}'";
                var result = await _context.Database.ExecuteSqlCommandAsync(command);

                //string password = MD5Encoder.MD5Hash(user.Password);
                //string command = $"dbo.Users_Upd @p_ID={user.Id},@p_USERNAME = {user.Username},@p_FULLNAME={user.Fullname},@p_PASSWORD={password},@p_EMAIL = {user.Email},@p_PHONE={user.Phone},@p_PARENT_ID={user.ParentId},@p_DESCRIPTION={user.Description},@p_RECORD_STATUS={user.RecordStatus},@p_AUTH_STATUS={user.AuthStatus},@p_CREATE_DT={user.CreateDt},@p_APPROVE_DT={user.ApproveDt},@p_EDIT_DT={user.EditDt},@p_MAKER_ID={user.MakerId},@p_CHECKER_ID={user.CheckerId},@p_EDITOR_ID={user.EditorId}";
                //var result = await context.Database.ExecuteSqlCommandAsync(command, cancellationToken: CancellationToken.None);
                rs.Succeeded = true;
                rs.Message = "Cài đặt đã được lưu.";
                ObjectResult objRes = new ObjectResult(rs);
                //context.Dispose();
                return objRes;
                //return result;
            }
            catch(Exception ex)
            {
                rs.Succeeded = false;
                rs.Message = "Lỗi " + ex.Message;
                ObjectResult objRes = new ObjectResult(rs);
                //context.Dispose();
                return objRes;
            }
        }
    }

}