using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Xml.Linq;
using ChatBot.Infrastructure.Core;
using Microsoft.AspNetCore.Authorization;

namespace ChatBot.Controllers
{
    [Produces("application/json")]
    [Route("api/Options")]
    public class OptionsController : Controller
    {
        private readonly DEFACEWEBSITEContext _context;
        public OptionsController(DEFACEWEBSITEContext context)
        {
            this._context = context;
        }
        [HttpGet]
        [Authorize(Roles = "ViewOption")]
        public async  Task<OptionSearchObject> Get(string domain)
        {
            
            //DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result = await _context.OptionSearchObject.FromSql("dbo.Option_Search @DOMAIN = {0} , @USERNAME='', @RECORD_STATUS = '',@IS_LIMIT ='',@TIMES='' ", domain).SingleAsync();
            //int a = 5;
            //context.Dispose();
            return result;
        }
        /*
         * @p_ID	int = NULL,
@p_DOMAIN_ID	varchar(500)  = NULL,
@p_IS_LIMIT	varchar(1)  = 'Y',
@p_TIMES	INT  = 1,
@p_DESCRIPTION	nvarchar(500)  = NULL,
@p_RECORD_STATUS	varchar(1)  = NULL,
@p_AUTH_STATUS	varchar(1)  = NULL,
@p_CREATE_DT	VARCHAR(20) = NULL,
@p_APPROVE_DT	VARCHAR(20) = NULL,
@p_EDIT_DT	VARCHAR(20) = NULL,
@p_MAKER_ID	varchar(15)  = NULL,
@p_CHECKER_ID	varchar(15)  = NULL,
@p_EDITOR_ID	varchar(15)  = NULL,
@DOMAINLINK XML = NULL,
@DOMAINUSER XML = NULL,
@IsEditUser varchar(1) = '0',
@IsEditLink varchar(1) = '0'
         */
        [HttpPut("{id}")]
        [Authorize(Roles = "EditOption")]
        public async Task<ObjectResult> Put(int id, [FromBody]OptionUpdateObject option)
        {
            GenericResult rs = new GenericResult();
            //DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            try
            {
                
                XElement xmlLink = new XElement(new XElement("Root"));
                XElement xmlUser = new XElement(new XElement("Root"));
                if (option.IsEditLink.Equals("1"))
                {
                    xmlUser = null;
                    xmlLink = new XElement(new XElement("Root"));
                    foreach (var item in option.DOMAINLINK)
                    {
                        XElement childElement = new XElement("Link", new XElement("OPTIONS_ID", option.OPTION.ID),
                                                        new XElement("DOMAIN_ID", option.OPTION.DOMAIN_ID),
                                                        new XElement("LINK", item.Link),
                                                        new XElement("RECORD_STATUS", "1"));
                        xmlLink.Add(childElement);
                    }
                    //List<OptionLinksNotify> listAddNotify = option.DOMAINLINK.Where(opt => opt.UserDomainNotify.IS_CCON.Equals("1") || opt.UserDomainNotify.IS_CCON.Equals("false")
                    //                                                                    || opt.UserDomainNotify.IS_CIP.Equals("1") || opt.UserDomainNotify.IS_CIP.Equals("false")
                    //                                                                    || opt.UserDomainNotify.IS_ECODE.Equals("1") || opt.UserDomainNotify.IS_ECODE.Equals("false")
                    //                                                                    || opt.UserDomainNotify.IS_RDOM.Equals("1") || opt.UserDomainNotify.IS_RDOM.Equals("false")
                    //                                                                    || opt.UserDomainNotify.IS_TOUT.Equals("1") || opt.UserDomainNotify.IS_TOUT.Equals("false")).ToList();
                    List<OptionLinksNotify> listAddNotify = option.DOMAINLINK;
                    if (listAddNotify.Count > 0)
                    {
                        XElement addNotifyXML = new XElement(new XElement("Root"));
                        foreach (var item in listAddNotify)
                        {
                            XElement childElement = new XElement("Link", new XElement("USERNAME", item.UserDomainNotify.USERNAME),
                                                                            new XElement("DOMAIN_ID", item.Link),
                                                                            new XElement("IS_TOUT", (item.UserDomainNotify.IS_TOUT.Equals("true") || item.UserDomainNotify.IS_TOUT.Equals("0")) ? 0 : 1),
                                                                            new XElement("IS_CIP", (item.UserDomainNotify.IS_CIP.Equals("true") || item.UserDomainNotify.IS_CIP.Equals("0")) ? 0 : 1),
                                                                            new XElement("IS_RDOM", (item.UserDomainNotify.IS_RDOM.Equals("true") || item.UserDomainNotify.IS_RDOM.Equals("0")) ? 0 : 1),
                                                                            new XElement("IS_ECODE", (item.UserDomainNotify.IS_ECODE.Equals("true") || item.UserDomainNotify.IS_ECODE.Equals("0")) ? 0 : 1),
                                                                            new XElement("IS_CCON", (item.UserDomainNotify.IS_CCON.Equals("true") || item.UserDomainNotify.IS_CCON.Equals("0")) ? 0 : 1));
                            addNotifyXML.Add(childElement);
                        }
                        string userDomainNotifyInsCommand = $"dbo.Userdomainnotify_Ins @USERNAME = '{option.OPTION.USERNAME}', @INSERTLINK = '{addNotifyXML}'";
                        var addResult = await _context.Database.ExecuteSqlCommandAsync(userDomainNotifyInsCommand);
                    }
                    //int a = 5;
                }
                if (option.IsEditUser.Equals("1"))
                {
                    xmlLink = null;
                    xmlUser = new XElement(new XElement("Root"));
                    foreach (var item in option.DOMAINUSER)
                    {
                        XElement childElement = new XElement("User", new XElement("DOMAIN", item.DOMAIN_ID),
                                                         new XElement("USERNAME", item.USER_ID),
                                                         new XElement("NOTES", item.NOTES));
                        xmlUser.Add(childElement);
                    }
                }

                string command = $"dbo.Options_Upd @p_ID= {option.OPTION.ID},@p_DOMAIN_ID ='{option.OPTION.DOMAIN_ID}',@p_IS_LIMIT ='{option.OPTION.IS_LIMIT}',@p_TIMES= {option.OPTION.TIMES},@p_DESCRIPTION = '{option.OPTION.DESCRIPTION}',@p_RECORD_STATUS = '{option.OPTION.RECORD_STATUS}',@p_AUTH_STATUS = '{option.OPTION.AUTH_STATUS}',@p_CREATE_DT = '{option.OPTION.CREATE_DT}',@p_APPROVE_DT = '{option.OPTION.APPROVE_DT}', @p_EDIT_DT ='{DateTime.Now.Date}',@p_MAKER_ID ='{option.OPTION.MAKER_ID}',@p_CHECKER_ID ='{option.OPTION.CHECKER_ID}',@p_EDITOR_ID = '{option.OPTION.EDITOR_ID}',@DOMAINLINK = '{xmlLink}',@DOMAINUSER= N'{xmlUser}',@IsEditUser = '{option.IsEditUser}',@IsEditLink = '{option.IsEditLink}' ";
                var result = await _context.Database.ExecuteSqlCommandAsync(command, cancellationToken: CancellationToken.None);
                rs.Succeeded = true;
                rs.Message = "Cập nhật thành công";
                ObjectResult objRes = new ObjectResult(rs);
                //context.Dispose();
                return objRes;
                //return result;
            }
            catch(Exception ex)
            {
                rs.Succeeded = false;
                rs.Message = "Lỗi " + ex.Message ; 
                ObjectResult objRes = new ObjectResult(rs);
                //context.Dispose();
                return objRes;
            }
        }
    }
}