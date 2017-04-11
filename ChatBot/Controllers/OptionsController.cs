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

namespace ChatBot.Controllers
{
    [Produces("application/json")]
    [Route("api/Options")]
    public class OptionsController : Controller
    {
        [HttpGet]
        public async Task<OptionSearchObject> Get(string domain)
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            var result = await context.OptionSearchObject.FromSql("dbo.Option_Search @DOMAIN = {0} , @USERNAME='', @RECORD_STATUS = '',@IS_LIMIT ='',@TIMES='' ", domain).SingleAsync();
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
        public async Task<int> Put(int id, [FromBody]OptionUpdateObject option)
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            XElement xmlLink = new XElement(new XElement("Root"));
            XElement xmlUser = new XElement(new XElement("Root"));
            if(option.IsEditLink.Equals("1"))
            {
                xmlUser = null;
                xmlLink = new XElement(new XElement("Root"));
                foreach(var item in option.DOMAINLINK)
                {
                    XElement childElement = new XElement("Link", new XElement("OPTIONS_ID", option.OPTION.ID),
                                                    new XElement("DOMAIN_ID", option.OPTION.DOMAIN_ID), 
                                                    new XElement("LINK", item.Link),
                                                    new XElement("RECORD_STATUS", "1"));
                    xmlLink.Add(childElement);
                }
            }
            if(option.IsEditUser.Equals("1"))
            {
                xmlLink = null;
                xmlUser = new XElement(new XElement("Root"));
                foreach(var item in option.DOMAINUSER)
                {
                   XElement childElement = new XElement("User", new XElement("DOMAIN", item.DOMAIN_ID),
                                                    new XElement("USERNAME", item.USERID),
                                                    new XElement("NOTE", item.NOTES));
                }
            }

            string command = $"dbo.Options_Upd @p_ID= {option.OPTION.ID},@p_DOMAIN_ID ='{option.OPTION.DOMAIN_ID}',@p_IS_LIMIT ='{option.OPTION.IS_LIMIT}',@p_TIMES= {option.OPTION.TIMES},@p_DESCRIPTION = '{option.OPTION.DESCRIPTION}',@p_RECORD_STATUS = '{option.OPTION.RECORD_STATUS}',@p_AUTH_STATUS = '{option.OPTION.AUTH_STATUS}',@p_CREATE_DT = '{option.OPTION.CREATE_DT}',@p_APPROVE_DT = '{option.OPTION.APPROVE_DT}', @p_EDIT_DT ='{option.OPTION.EDIT_DT}',@p_MAKER_ID ='{option.OPTION.MAKER_ID}',@p_CHECKER_ID ='{option.OPTION.CHECKER_ID}',@p_EDITOR_ID = '{option.OPTION.EDITOR_ID}',@DOMAINLINK = '{xmlLink}',@DOMAINUSER= '{xmlUser}',@IsEditUser = '{option.IsEditUser}',@IsEditLink = '{option.IsEditLink}' ";
            var result = await context.Database.ExecuteSqlCommandAsync(command,cancellationToken:CancellationToken.None);
            return result;
        }
    }
}